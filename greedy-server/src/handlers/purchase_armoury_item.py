import dataclasses
import datetime as dt

from bson import ObjectId
from fastapi import Depends

from src.auth import AuthenticatedRequestContext, get_authenticated_context
from src.handlers.abc import HandlerException
from src.mongo.armoury import (ArmouryItemModel, ArmouryRepository,
                               armoury_repository)
from src.mongo.bountyshop import BountyShopRepository, bountyshop_repository
from src.mongo.currency import CurrenciesModel, CurrencyRepository
from src.mongo.currency import Fields as CurrencyRepoFields
from src.mongo.currency import currency_repository
from src.static_models.bountyshop import (BountyShopArmouryItem,
                                          DynamicBountyShop,
                                          dynamic_bounty_shop)

from .basepurchasehandler import BaseBountyShopPurchaseHandler


@dataclasses.dataclass()
class PurchaseArmouryItemResponse:
    currencies: CurrenciesModel
    purchase_cost: int
    item: ArmouryItemModel


class PurchaseArmouryItemHandler(BaseBountyShopPurchaseHandler):
    def __init__(
        self,
        ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
        currency_repo: CurrencyRepository = Depends(currency_repository),
        armoury_repo: ArmouryRepository = Depends(armoury_repository),
        bountyshop_repo: BountyShopRepository = Depends(bountyshop_repository),
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

        num_purchases: int = await self.get_item_purchase_count(uid, item_id, self.prev_reset)

        currencies: CurrenciesModel = await self.currency_repo.get_user(uid)

        # User has reached the purchase limit
        if num_purchases >= item.purchase_limit:
            raise HandlerException(400, "Item unavailable")

        # Verify that the user can afford to purchase the item
        elif item.purchase_cost > currencies.bounty_points:
            raise HandlerException(400, "Cannot afford item")

        try:
            currencies = await self.currency_repo.inc_value(uid, CurrencyRepoFields.BOUNTY_POINTS, -item.purchase_cost)

            armoury_item: ArmouryItemModel = await self.armoury_repo.inc_item_owned(uid, item.armoury_item_id, 1)

        finally:
            await self.log_purchase(uid, item)

        return PurchaseArmouryItemResponse(currencies=currencies, purchase_cost=item.purchase_cost, item=armoury_item)
