from src.routing.handlers.abc import BaseHandler, BaseHandlerException
import dataclasses
from fastapi import Depends

from src.mongo.repositories.bounties import (BountiesRepository, UserBountiesModel, inject_bounties_repository)
from src.mongo.repositories.currency import CurrencyRepository
from src.mongo.repositories.currency import inject_currency_repository
from src.resources.bounties import StaticBounties, inject_static_bounties
from src.authentication.authentication import AuthenticatedUser


@dataclasses.dataclass()
class UpdateBountiesResponse:
    bounties: UserBountiesModel


class UpdateBountiesException(BaseHandlerException):
    ...


class UpdateBountiesHandler(BaseHandler):
    def __init__(
            self,
            static_bounties: StaticBounties = Depends(inject_static_bounties),
            bounties_repo: BountiesRepository = Depends(inject_bounties_repository),
            currency_repo: CurrencyRepository = Depends(inject_currency_repository),
    ):
        self.bounties_data = static_bounties
        self.bounties_repo = bounties_repo
        self.currency_repo = currency_repo

    async def handle(self, user: AuthenticatedUser, bounty_ids: list[int]) -> UpdateBountiesResponse:
        # Check that the user is attempting to activate an acceptable num of bounties
        self.check_num_active_bounties(bounty_ids)

        # Load data from the mongo database
        user_bounty_data: UserBountiesModel = await self.bounties_repo.get_user(user.id)

        self.check_num_active_bounties(bounty_ids)
        self.check_unlocked_bounty(bounty_ids, user_bounty_data)

        # Enable (or disable) the relevant bounties
        await self.bounties_repo.update_active_bounties(user.id, bounty_ids)

        return UpdateBountiesResponse(
            bounties=await self.bounties_repo.get_user(user.id)
        )

    def check_num_active_bounties(self, bounty_ids: list[int]):
        if len(bounty_ids) > self.bounties_data.max_active_bounties:
            raise UpdateBountiesException()

        return True

    @staticmethod
    def check_unlocked_bounty(bounty_ids: list[int], user_data: UserBountiesModel):
        user_bounty_ids = [b.bounty_id for b in user_data.bounties]

        if not all(id_ in user_bounty_ids for id_ in bounty_ids):
            raise UpdateBountiesException()

        return True
