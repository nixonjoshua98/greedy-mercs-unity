import dataclasses
import datetime as dt
from typing import Optional

from bson import ObjectId
from fastapi import Depends

from src.auth import AuthenticatedRequestContext, get_authenticated_context
from src.common.types import CurrencyType
from src.handlers.abc import HandlerException
from src.mongo.bountyshop import BountyShopRepository, bountyshop_repository
from src.mongo.currency import CurrenciesModel, CurrencyRepository
from src.mongo.currency import Fields as CurrencyRepoFields
from src.mongo.currency import currency_repository
from src.static_models.bountyshop import (BountyShopCurrencyItem,
                                          DynamicBountyShop,
                                          dynamic_bounty_shop)

from .basepurchasehandler import BaseBountyShopPurchaseHandler


@dataclasses.dataclass()
class PurchaseCurrencyResponse:
    currencies: CurrenciesModel
    purchase_cost: int
    currency_gained: int


class PurchaseCurrencyHandler(BaseBountyShopPurchaseHandler):
    def __init__(
        self,
        ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
        currency_repo: CurrencyRepository = Depends(currency_repository),
        bountyshop_repo: BountyShopRepository = Depends(bountyshop_repository),
        bounty_shop: DynamicBountyShop = Depends(dynamic_bounty_shop),
    ):
        self.datetime: dt.datetime = ctx.datetime
        self.prev_reset: dt.datetime = ctx.prev_daily_reset

        self.shop = bounty_shop
        self.shop_repo = bountyshop_repo
        self.currency_repo = currency_repo

    async def handle(self, uid: ObjectId, item_id: str) -> PurchaseCurrencyResponse:
        item: BountyShopCurrencyItem = self.shop.get_item(item_id)

        if not isinstance(item, BountyShopCurrencyItem):
            raise HandlerException(400, "Invalid item")

        elif (item_field := self._id_to_field(item.currency_type)) is None:
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
            currencies = await self.currency_repo.inc_values(uid, {
                CurrencyRepoFields.BOUNTY_POINTS: -item.purchase_cost,
                item_field: item.purchase_quantity
            })

        finally:
            await self.log_purchase(uid, item)

        return PurchaseCurrencyResponse(
            currencies=currencies,
            purchase_cost=item.purchase_cost,
            currency_gained=item.purchase_quantity
        )

    @staticmethod
    def _id_to_field(currency: int) -> Optional[str]:
        return {
            CurrencyType.ARMOURY_POINTS: CurrencyRepoFields.ARMOURY_POINTS
        }.get(currency)
