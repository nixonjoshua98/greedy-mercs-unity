import dataclasses
import datetime as dt
import math

from fastapi import Depends

from src import utils
from src.authentication.authentication import AuthenticatedUser
from src.mongo.repositories.bounties import (BountiesRepository,
                                             UserBountiesModel,
                                             inject_bounties_repository)
from src.mongo.repositories.currency import CurrenciesModel, CurrencyRepository
from src.mongo.repositories.currency import Fields as CurrencyFields
from src.mongo.repositories.currency import inject_currency_repository
from src.resources.bounties import StaticBounties, inject_static_bounties
from src.routing.handlers.abc import BaseHandler, BaseResponse


@dataclasses.dataclass()
class BountyClaimResponse(BaseResponse):
    claim_time: dt.datetime
    claim_amount: int
    currencies: CurrenciesModel


class BountyClaimHandler(BaseHandler):
    def __init__(
            self,
            static_bounties: StaticBounties = Depends(inject_static_bounties),
            bounties_repo: BountiesRepository = Depends(inject_bounties_repository),
            currency_repo: CurrencyRepository = Depends(inject_currency_repository),
    ):
        self.bounties_data = static_bounties
        self.bounties_repo = bounties_repo
        self.currency_repo = currency_repo

    async def handle(self, user: AuthenticatedUser) -> BountyClaimResponse:
        """
        Perform the 'Bounty Claim' for the provided user

        :param user: Authenticated user we are performing the claim for
        """

        # We use the current server time for the claim
        claim_time = dt.datetime.utcnow()

        # Fetch bounties data for the user
        user_bounties: UserBountiesModel = await self.bounties_repo.get_user_bounties(user.id)

        # Calculate the total unclaimed points
        points = self.unclaimed_points(claim_time, user_bounties)

        # Update the users' claim time
        await self.bounties_repo.set_claim_time(user.id, claim_time)

        # Increment the currency and fetch the updated document
        currencies = await self.currency_repo.increment_value(user.id, CurrencyFields.BOUNTY_POINTS, points)

        return BountyClaimResponse(claim_time=claim_time, claim_amount=points, currencies=currencies)

    def unclaimed_points(self, now: dt.datetime, user_bounties: UserBountiesModel) -> int:
        points = 0  # Total unclaimed points (ready to be claimed)

        # Interate over each active bounty available
        for bounty in user_bounties.active_bounties:
            s_bounty_data = utils.get(self.bounties_data.bounties, id=bounty.bounty_id)

            # Num. hours since the user has claimed this bounty
            total_hours = (now - user_bounties.last_claim_time).total_seconds() / 3_600

            # Clamp between 0 - max_unclaimed_hours
            hours_clamped = max(0, min(self.bounties_data.max_unclaimed_hours, total_hours))  # type: ignore

            # Calculate the income and increment the total
            points += math.floor(hours_clamped * s_bounty_data.income)

        return points
