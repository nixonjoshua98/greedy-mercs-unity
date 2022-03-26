from fastapi import Depends

from src.context import AuthenticatedRequestContext
from src.dependencies import get_static_bounties
from src.exceptions import HandlerException
from src.repositories.bounties import (BountiesRepository,
                                       UserBountiesDataModel,
                                       get_bounties_repository)
from src.repositories.currency import (CurrencyRepository,
                                       get_currency_repository)
from src.shared_models import BaseModel
from src.static_models.bounties import StaticBounties


class UpdateBountiesResponse(BaseModel):
    bounties: UserBountiesDataModel


class UpdateBountiesHandler:
    def __init__(
        self,
        # Static Data #
        static_bounties=Depends(get_static_bounties),
        # Repositories #
        bounties_repo: BountiesRepository = Depends(get_bounties_repository),
        currency_repo: CurrencyRepository = Depends(get_currency_repository),
    ):
        self.bounties_data: StaticBounties = static_bounties
        self.bounties_repo = bounties_repo
        self.currency_repo = currency_repo

    async def handle(self, user: AuthenticatedRequestContext, bounty_ids: list[int]) -> UpdateBountiesResponse:

        if len(bounty_ids) > self.bounties_data.max_active_bounties:
            raise HandlerException(400, "Exceeded maximum active bounties")

        # Load data from the mongo database
        user_bounty_data: UserBountiesDataModel = (
            await self.bounties_repo.get_user_bounties(user.user_id)
        )

        if not self.is_bounties_valid(bounty_ids, user_bounty_data):
            raise HandlerException(400, "Attempting to use locked or invalid bounty")

        # Enable (or disable) the relevant bounties
        await self.bounties_repo.update_active_bounties(user.user_id, bounty_ids)

        return UpdateBountiesResponse(bounties=await self.bounties_repo.get_user_bounties(user.user_id))

    def is_bounties_valid(self, bounty_ids: list[int], user_data: UserBountiesDataModel) -> bool:
        u_bounty_ids: list[int] = [b.bounty_id for b in user_data.bounties]
        s_bounty_ids: list[int] = [b.id for b in self.bounties_data.bounties]

        return all(id_ in u_bounty_ids and id_ in s_bounty_ids for id_ in bounty_ids)
