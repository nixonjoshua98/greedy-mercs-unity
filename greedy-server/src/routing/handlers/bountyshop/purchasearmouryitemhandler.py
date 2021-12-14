import dataclasses
import datetime as dt
from bson import ObjectId
from fastapi import Depends
from src.request_context import AuthenticatedRequestContext, authenticated_context

from src.mongo.repositories.armoury import (
    ArmouryItemModel,
    ArmouryRepository,
    armoury_repository,
)
from src.mongo.repositories.bountyshop import (
    BountyShopRepository,
    inject_bountyshop_repo,
)
from src.mongo.repositories.currency import CurrenciesModel, CurrencyRepository
from src.mongo.repositories.currency import Fields as CurrencyRepoFields
from src.mongo.repositories.currency import currency_repository
from src.resources.bountyshop import (
    BountyShopArmouryItem,
    dynamic_bounty_shop,
    DynamicBountyShop,
)
from src.routing.handlers.abc import BaseHandler, HandlerException


@dataclasses.dataclass()
class PurchaseArmouryItemResponse:
    currencies: CurrenciesModel
    purchase_cost: int
    item: ArmouryItemModel


class PurchaseArmouryItemHandler(BaseHandler):
    def __init__(
        self,
        ctx: AuthenticatedRequestContext = Depends(authenticated_context),
        currency_repo: CurrencyRepository = Depends(currency_repository),
        armoury_repo: ArmouryRepository = Depends(armoury_repository),
        bountyshop_repo: BountyShopRepository = Depends(inject_bountyshop_repo),
        bounty_shop: DynamicBountyShop = Depends(dynamic_bounty_shop),
    ):
        self.datetime: dt.datetime = ctx.datetime
        self.prev_reset: dt.datetime = ctx.prev_daily_reset

        self.shop = bounty_shop
        self.shop_repo = bountyshop_repo
        self.currency_repo = currency_repo
        self.armoury_repo = armoury_repo

    async def handle(self, uid: ObjectId, item_id: str) -> PurchaseArmouryItemResponse:
        item: BountyShopArmouryItem = self.shop.get_item(item_id)

        # Item type is not what we expected
        if not isinstance(item, BountyShopArmouryItem):
            raise HandlerException(400, "Invalid item")

        num_purchases: int = await self.get_item_purchase_count(
            uid, item_id, self.prev_reset
        )
        currencies: CurrenciesModel = await self.currency_repo.get_user(uid)

        # User has reached the purchase limit
        if num_purchases >= item.purchase_limit:
            raise HandlerException(400, "Item unavailable")

        # Verify that the user can afford to purchase the item
        if (purchase_cost := self.purchase_cost(item)) > currencies.bounty_points:
            raise HandlerException(400, "Cannot afford item")

        try:
            currencies = await self.currency_repo.inc_value(
                uid, CurrencyRepoFields.BOUNTY_POINTS, -purchase_cost
            )

            armoury_item: ArmouryItemModel = await self.armoury_repo.inc_item_owned(
                uid, item.armoury_item, 1
            )

        finally:  # Always log the purchase since we do not know what the error was here
            await self.log_purchase(uid, item, purchase_cost)

        return PurchaseArmouryItemResponse(
            currencies=currencies, purchase_cost=purchase_cost, item=armoury_item
        )

    async def log_purchase(self, uid: ObjectId, item: BountyShopArmouryItem, cost: int):
        await self.shop_repo.add_purchase(uid, item.id, self.prev_reset, cost)

    async def get_item_purchase_count(
        self, uid: ObjectId, item_id: str, prev_reset: dt.datetime
    ):
        purchases = await self.shop_repo.get_daily_item_purchases(
            uid, item_id, prev_reset
        )

        return len(purchases)

    @staticmethod
    def purchase_cost(item: BountyShopArmouryItem) -> int:
        return item.purchase_cost
