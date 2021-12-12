import dataclasses

from fastapi import Depends

from src import utils
from src.authentication.authentication import AuthenticatedUser
from src.mongo.repositories.armoury import ArmouryItemModel, ArmouryRepository
from src.mongo.repositories.armoury import Fields as ArmouryFields
from src.mongo.repositories.armoury import armoury_repository
from src.mongo.repositories.currency import (CurrencyRepository,
                                             currency_repository)
from src.resources.armoury import StaticArmouryItem, static_armoury
from src.routing.handlers.abc import (BaseHandler, BaseResponse,
                                      HandlerException)


@dataclasses.dataclass()
class MergeItemResponse(BaseResponse):
    item: ArmouryItemModel


class MergeItemHandler(BaseHandler):
    def __init__(
            self,
            armoury_data: list[StaticArmouryItem] = Depends(static_armoury),
            armoury_repo: ArmouryRepository = Depends(armoury_repository),
            currency_repo: CurrencyRepository = Depends(currency_repository),
    ):
        self.armoury_data = armoury_data
        self.armoury_repo = armoury_repo
        self.currency_repo = currency_repo

    async def handle(self, user: AuthenticatedUser, item_id: int) -> MergeItemResponse:

        s_item: StaticArmouryItem = utils.get(self.armoury_data, id=item_id)
        u_item: ArmouryItemModel = await self.armoury_repo.get_user_item(user.id, item_id)

        # Item is either invalid or locked
        if s_item is None or u_item is None:
            raise HandlerException(400, "Failed to upgrade locked/invalid item")

        # At or will exceed the max merge level
        elif (u_item.merge_lvl + 1) > s_item.max_merge_lvl:
            raise HandlerException(400, "Item is at or will exceed max level")

        # Calculate the upgrade cost for the item
        merge_cost = self.merge_cost(s_item, u_item)

        # User cannot afford the upgrade cost
        if merge_cost > u_item.owned:
            raise HandlerException(400, "Cannot afford merge cost")

        u_item = await self.perform_merge(user.id, item_id, 1, merge_cost)

        return MergeItemResponse(item=u_item)

    async def perform_merge(self, user_id, iid: int, merge_level: int, merge_cost: int):
        return await self.armoury_repo.update_item(user_id, iid, {
            "$inc": {
                ArmouryFields.MERGE_LEVEL: merge_level,
                ArmouryFields.NUM_OWNED: -merge_cost,
            }
        }, upsert=False)

    @staticmethod
    def merge_cost(s_item: StaticArmouryItem, item: ArmouryItemModel) -> int:
        return s_item.base_merge_cost
