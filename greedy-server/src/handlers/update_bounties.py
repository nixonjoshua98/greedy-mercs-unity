import dataclasses

from fastapi import Depends

from src.context import AuthenticatedRequestContext
from src.handlers.abc import BaseHandler, HandlerException
from src.mongo.repositories.bounties import (BountiesRepository,
                                             UserBountiesDataModel,
                                             bounties_repository)
from src.mongo.repositories.currency import (CurrencyRepository,
                                             currency_repository)
from src.resources.bounties import StaticBounties, inject_static_bounties


@dataclasses.dataclass()
class UpdateBountiesResponse:
    bounties: UserBountiesDataModel


class UpdateBountiesHandler(BaseHandler):
    def __init__(
        self,
        static_bounties: StaticBounties = Depends(inject_static_bounties),
        bounties_repo: BountiesRepository = Depends(bounties_repository),
        currency_repo: CurrencyRepository = Depends(currency_repository),
    ):
        self.bounties_data = static_bounties
        self.bounties_repo = bounties_repo
        self.currency_repo = currency_repo

    async def handle(
        self, user: AuthenticatedRequestContext, bounty_ids: list[int]
    ) -> UpdateBountiesResponse:

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

        bounties: UserBountiesDataModel = await self.bounties_repo.get_user_bounties(
            user.user_id
        )

        return UpdateBountiesResponse(bounties=bounties)

    def is_bounties_valid(self, bounty_ids: list[int], user_data: UserBountiesDataModel) -> bool:
        u_bounty_ids: list[int] = [b.bounty_id for b in user_data.bounties]
        s_bounty_ids: list[int] = [b.id for b in self.bounties_data.bounties]

        return all(id_ in u_bounty_ids and id_ in s_bounty_ids for id_ in bounty_ids)