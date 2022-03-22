
import datetime as dt
import math

from fastapi import Depends

from src import utils
from src.context import AuthenticatedRequestContext, RequestContext
from src.dependencies import get_static_bounties
from src.handlers.auth_handler import get_authenticated_context
from src.repositories.bounties import (BountiesRepository,
                                       UserBountiesDataModel,
                                       get_bounties_repository)
from src.repositories.currency import CurrenciesModel, CurrencyRepository
from src.repositories.currency import Fields as CurrencyFields
from src.repositories.currency import get_currency_repository
from src.shared_models import BaseModel
from src.static_models.bounties import StaticBounties


class BountyClaimResponse(BaseModel):
    claim_time: dt.datetime
    claim_amount: int
    currencies: CurrenciesModel


class ClaimBountiesHandler:
    def __init__(
        self,
        bounties_data: StaticBounties = Depends(get_static_bounties),
        bounties_repo: BountiesRepository = Depends(get_bounties_repository),
        currency_repo: CurrencyRepository = Depends(get_currency_repository),
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

        # Update the users' claim time
        await self.bounties_repo.set_claim_time(ctx.user_id, claim_time)

        # Increment the currency and fetch the updated document
        currencies = await self.currency_repo.incr(ctx.user_id, CurrencyFields.bounty_points, points)

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
