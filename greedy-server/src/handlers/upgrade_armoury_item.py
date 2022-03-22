

from fastapi import Depends

from src import utils
from src.context import AuthenticatedRequestContext, RequestContext
from src.dependencies import get_static_armoury
from src.exceptions import HandlerException
from src.handlers.auth_handler import get_authenticated_context
from src.repositories.armoury import (ArmouryItemModel, ArmouryRepository,
                                      get_armoury_repository)
from src.repositories.currency import CurrenciesModel, CurrencyRepository
from src.repositories.currency import Fields as CurrencyFields
from src.repositories.currency import get_currency_repository
from src.shared_models import BaseModel
from src.static_models.armoury import ArmouryItemID, StaticArmouryItem


class UpgradeItemResponse(BaseModel):
    item: ArmouryItemModel
    currencies: CurrenciesModel
    upgrade_cost: int


class UpgradeArmouryItemHandler:
    def __init__(
        self,
        static_data=Depends(get_static_armoury),
        armoury_repo=Depends(get_armoury_repository),
        currency_repo=Depends(get_currency_repository),
    ):
        self.static_data: list[StaticArmouryItem]= static_data

        self.armoury_repo: ArmouryRepository = armoury_repo
        self.currency_repo: CurrencyRepository = currency_repo

    async def handle(self, user: AuthenticatedRequestContext, item_id: ArmouryItemID) -> UpgradeItemResponse:

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
        u_currencies = await self.currency_repo.incr(
            user.user_id, CurrencyFields.armoury_points, -upgrade_cost
        )

        # Update the requested item here
        user_item = await self.armoury_repo.inc_item_level(user.user_id, item_id, 1)

        return UpgradeItemResponse(
            item=user_item, currencies=u_currencies, upgrade_cost=upgrade_cost
        )

    @staticmethod
    def upgrade_cost(s_item: StaticArmouryItem, item: ArmouryItemModel) -> int:
        return 5 + item.level
