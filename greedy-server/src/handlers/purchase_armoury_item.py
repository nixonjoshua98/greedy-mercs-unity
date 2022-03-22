
import datetime as dt

from bson import ObjectId
from fastapi import Depends

from src.context import AuthenticatedRequestContext, RequestContext
from src.exceptions import HandlerException
from src.handlers.auth_handler import get_authenticated_context
from src.repositories.armoury import (ArmouryItemModel, ArmouryRepository,
                                      get_armoury_repository)
from src.repositories.bountyshop import (BountyShopRepository,
                                         bountyshop_repository)
from src.repositories.currency import CurrenciesModel, CurrencyRepository
from src.repositories.currency import Fields as CurrencyRepoFields
from src.repositories.currency import get_currency_repository
from src.shared_models import BaseModel
from src.static_models.bountyshop import (BountyShopArmouryItem,
                                          DynamicBountyShop,
                                          dynamic_bounty_shop)


class PurchaseArmouryItemResponse(BaseModel):
    currencies: CurrenciesModel
    purchase_cost: int
    item: ArmouryItemModel


class PurchaseArmouryItemHandler:
    def __init__(
        self,
        ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
        currency_repo: CurrencyRepository = Depends(get_currency_repository),
        armoury_repo: ArmouryRepository = Depends(get_armoury_repository),
        bountyshop_repo: BountyShopRepository = Depends(bountyshop_repository),
        bounty_shop: DynamicBountyShop = Depends(dynamic_bounty_shop),
    ):
        self.datetime: dt.datetime = ctx.datetime
        self.prev_reset: dt.datetime = ctx.prev_daily_refresh

        self.shop = bounty_shop
        self.shop_repo = bountyshop_repo
        self.currency_repo = currency_repo
        self.armoury_repo = armoury_repo

    async def log_purchase(self, uid: ObjectId, item):
        await self.shop_repo.add_purchase(uid, item.id, self.prev_reset, item.purchase_cost)

    async def get_item_purchase_count(self, uid: ObjectId, item_id: str, prev_reset: dt.datetime) -> int:
        purchases = await self.shop_repo.get_daily_item_purchases(uid, item_id, prev_reset)

        return len(purchases)

    async def handle(self, uid: ObjectId, item_id: str) -> PurchaseArmouryItemResponse:
        item: BountyShopArmouryItem = self.shop.get_item(item_id)

        # Item type is not what we expected
        if not isinstance(item, BountyShopArmouryItem):
            raise HandlerException(400, "Invalid item")

        num_purchases: int = await self.get_item_purchase_count(uid, item_id, self.prev_reset)

        currencies: CurrenciesModel = await self.currency_repo.get_user(uid)

        # User has reached the purchase limit
        if num_purchases >= item.purchase_limit:
            raise HandlerException(400, "Item unavailable")

        # Verify that the user can afford to purchase the item
        elif item.purchase_cost > currencies.bounty_points:
            raise HandlerException(400, "Cannot afford item")

        try:
            currencies = await self.currency_repo.incr(uid, CurrencyRepoFields.bounty_points, -item.purchase_cost)

            armoury_item: ArmouryItemModel = await self.armoury_repo.inc_item_owned(uid, item.armoury_item_id, 1)

        finally:
            await self.log_purchase(uid, item)

        return PurchaseArmouryItemResponse(currencies=currencies, purchase_cost=item.purchase_cost, item=armoury_item)
