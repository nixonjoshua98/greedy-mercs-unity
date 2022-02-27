import dataclasses

from fastapi import Depends

from src import utils
from src.context import AuthenticatedRequestContext
from src.handlers.abc import BaseHandler, BaseResponse, HandlerException
from src.mongo.repositories.armoury import (ArmouryItemModel,
                                            ArmouryRepository,
                                            armoury_repository)
from src.mongo.repositories.currency import CurrenciesModel, CurrencyRepository
from src.mongo.repositories.currency import Fields as CurrencyFields
from src.mongo.repositories.currency import currency_repository
from src.resources.armoury import StaticArmouryItem, static_armoury


@dataclasses.dataclass()
class UpgradeItemResponse(BaseResponse):
    item: ArmouryItemModel
    currencies: CurrenciesModel
    upgrade_cost: int


class UpgradeItemHandler(BaseHandler):
    def __init__(
        self,
        static_data: list[StaticArmouryItem] = Depends(static_armoury),
        armoury_repo: ArmouryRepository = Depends(armoury_repository),
        currency_repo: CurrencyRepository = Depends(currency_repository),
    ):
        self.static_data = static_data

        self.armoury_repo = armoury_repo
        self.currency_repo = currency_repo

    async def handle(self, user: AuthenticatedRequestContext, item_id: int) -> UpgradeItemResponse:

        static_item: StaticArmouryItem = utils.get(self.static_data, id=item_id)
        user_item: ArmouryItemModel = await self.armoury_repo.get_user_item(user.user_id, item_id)

        # Item is either invalid or locked
        if static_item is None or user_item is None:
            raise HandlerException(400, "Failed to upgrade locked/invalid item")

        # Calculate the upgrade cost for the item
        upgrade_cost = self.upgrade_cost(static_item, user_item)

        # Fetch the currency we need
        u_currencies = await self.currency_repo.get_user(user.user_id)

        # User cannot afford the upgrade cost
        if upgrade_cost > u_currencies.armoury_points:
            raise HandlerException(400, "Cannot afford upgrade cost")

        # Deduct the upgrade cost and return all user items AFTER the update
        u_currencies = await self.currency_repo.inc_value(
            user.user_id, CurrencyFields.ARMOURY_POINTS, -upgrade_cost
        )

        # Update the requested item here
        user_item = await self.armoury_repo.inc_item_level(user.user_id, item_id, 1)

        return UpgradeItemResponse(
            item=user_item, currencies=u_currencies, upgrade_cost=upgrade_cost
        )

    @staticmethod
    def upgrade_cost(s_item: StaticArmouryItem, item: ArmouryItemModel) -> int:
        return 5 + item.level