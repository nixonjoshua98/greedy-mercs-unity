import dataclasses
import datetime as dt
import math

from fastapi import Depends

from src import utils
from src.auth import AuthenticatedRequestContext
from src.dependencies import get_static_bounties
from src.handlers.abc import BaseHandler, BaseResponse, HandlerException
from src.mongo.bounties import (BountiesRepository, UserBountiesDataModel,
                                bounties_repository)
from src.mongo.currency import CurrenciesModel, CurrencyRepository
from src.mongo.currency import Fields as CurrencyFields
from src.mongo.currency import currency_repository
from src.static_models.bounties import StaticBounties


@dataclasses.dataclass()
class BountyClaimResponse(BaseResponse):
    claim_time: dt.datetime
    claim_amount: int
    currencies: CurrenciesModel


class ClaimBountiesHandler(BaseHandler):
    def __init__(
        self,
        bounties_data: StaticBounties = Depends(get_static_bounties),
        bounties_repo: BountiesRepository = Depends(bounties_repository),
        currency_repo: CurrencyRepository = Depends(currency_repository),
    ):
        self.bounties_data = bounties_data
        self.bounties_repo = bounties_repo
        self.currency_repo = currency_repo

    async def handle(self, ctx: AuthenticatedRequestContext) -> BountyClaimResponse:
        claim_time = ctx.datetime

        # Fetch bounties data for the user
        user_bounties: UserBountiesDataModel = await self.bounties_repo.get_user_bounties(ctx.user_id)

        # Calculate the total unclaimed points
        points = self.unclaimed_points(claim_time, user_bounties)

        if points <= 0:
            raise HandlerException(400, "Claim points cannot be zero")

        # Update the users' claim time
        await self.bounties_repo.set_claim_time(ctx.user_id, claim_time)

        # Increment the currency and fetch the updated document
        currencies = await self.currency_repo.inc_value(ctx.user_id, CurrencyFields.BOUNTY_POINTS, points)

        return BountyClaimResponse(claim_time=claim_time, claim_amount=points, currencies=currencies)

    def unclaimed_points(self, now: dt.datetime, user_bounties: UserBountiesDataModel) -> int:
        points = 0  # Total unclaimed points (ready to be claimed)

        # Interate over each active bounty available
        for bounty in user_bounties.active_bounties:
            s_bounty_data = utils.get(self.bounties_data.bounties, id=bounty.bounty_id)

            # Num. hours since the user has claimed this bounty
            total_hours = (now - user_bounties.last_claim_time).total_seconds() / 3_600

            # Clamp between 0 - max_unclaimed_hours
            hours_clamped = max(0, min(self.bounties_data.max_unclaimed_hours, total_hours))  # type: ignore

            # Calculate the income and increment the total
            points += hours_clamped * s_bounty_data.income

        return math.floor(points)
