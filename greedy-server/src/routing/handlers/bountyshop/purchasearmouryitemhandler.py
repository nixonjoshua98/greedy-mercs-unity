import dataclasses

from bson import ObjectId
from fastapi import Depends
import datetime as dt
from src.mongo.repositories.armoury import (ArmouryItemModel,
                                            ArmouryRepository,
                                            armoury_repository)
from src.mongo.repositories.currency import CurrenciesModel, CurrencyRepository
from src.mongo.repositories.currency import Fields as CurrencyRepoFields
from src.mongo.repositories.currency import currency_repository
from src.resources.bountyshop import BountyShopArmouryItem, dynamic_bounty_shop
from src.routing.handlers.abc import BaseHandler, HandlerException
from src.mongo.repositories.bountyshop import BountyShopRepository, inject_bountyshop_repo, BountyShopPurchaseModel


@dataclasses.dataclass()
class PurchaseArmouryItemResponse:
    currencies: CurrenciesModel
    purchase_cost: int
    item: ArmouryItemModel


class PurchaseArmouryItemHandler(BaseHandler):
    def __init__(
            self,
            currency_repo: CurrencyRepository = Depends(currency_repository),
            armoury_repo: ArmouryRepository = Depends(armoury_repository),
            bountyshop_repo: BountyShopRepository = Depends(inject_bountyshop_repo),
            bounty_shop=Depends(dynamic_bounty_shop),
    ):
        self.shop = bounty_shop
        self.shop_repo = bountyshop_repo
        self.currency_repo = currency_repo
        self.armoury_repo = armoury_repo

    async def handle(self, user_id: ObjectId, item_id: str, prev_reset: dt.datetime) -> PurchaseArmouryItemResponse:
        item: BountyShopArmouryItem = self.shop.get_item(item_id)

        if not isinstance(item, BountyShopArmouryItem):
            raise HandlerException(400, "Invalid item")

        item_purchases: list[BountyShopPurchaseModel] = await self.shop_repo.get_daily_item_purchases(
            user_id, item_id, prev_reset
        )
        num_purchases = len(item_purchases)

        currencies: CurrenciesModel = await self.currency_repo.get_user(user_id)

        # Check the user still has 'stock' left
        if num_purchases >= item.purchase_limit:
            raise HandlerException(400, "Item unavailable")

        # Verify that the user can afford to purchase the item
        if (purchase_cost := self.purchase_cost(item)) > currencies.bounty_points:
            raise HandlerException(400, "Cannot afford item")

        try:
            currencies = await self.currency_repo.inc_value(user_id, CurrencyRepoFields.BOUNTY_POINTS, -purchase_cost)

            armoury_item: ArmouryItemModel = await self.armoury_repo.inc_item_owned(user_id, item.armoury_item, 1)

        finally:
            await self.shop_repo.add_purchase(user_id, item_id, prev_reset)

        return PurchaseArmouryItemResponse(currencies=currencies, purchase_cost=purchase_cost, item=armoury_item)

    @staticmethod
    def purchase_cost(item: BountyShopArmouryItem) -> int:
        return item.purchase_cost
