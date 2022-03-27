
import datetime as dt
from typing import Optional

from bson import ObjectId
from fastapi import Depends

from src.common.types import CurrencyType
from src.context import AuthenticatedRequestContext
from src.exceptions import HandlerException
from src.handlers.auth_handler import get_authenticated_context
from src.repositories.bountyshop import (BountyShopRepository,
                                         bountyshop_repository)
from src.repositories.currency import CurrenciesModel, CurrencyRepository
from src.repositories.currency import Fields as CurrencyRepoFields
from src.repositories.currency import get_currency_repository
from src.shared_models import BaseModel
from src.static_models.bountyshop import (BountyShopCurrencyItem,
                                          DynamicBountyShop,
                                          dynamic_bounty_shop)


class PurchaseCurrencyResponse(BaseModel):
    currencies: CurrenciesModel
    purchase_cost: int
    currency_gained: int


class PurchaseCurrencyHandler:
    def __init__(
        self,
        ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
        currency_repo: CurrencyRepository = Depends(get_currency_repository),
        bountyshop_repo: BountyShopRepository = Depends(bountyshop_repository),
        bounty_shop: DynamicBountyShop = Depends(dynamic_bounty_shop),
    ):
        self.datetime: dt.datetime = ctx.datetime
        self.prev_reset: dt.datetime = ctx.prev_daily_refresh

        self.shop = bounty_shop
        self.shop_repo = bountyshop_repo
        self.currency_repo = currency_repo

    async def log_purchase(self, uid: ObjectId, item):
        await self.shop_repo.add_purchase(uid, item.id, self.prev_reset, item.purchase_cost)

    async def get_item_purchase_count(self, uid: ObjectId, item_id: str, prev_reset: dt.datetime) -> int:
        purchases = await self.shop_repo.get_daily_item_purchases(uid, item_id, prev_reset)

        return len(purchases)

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
                CurrencyRepoFields.bounty_points: -item.purchase_cost,
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
            CurrencyType.ARMOURY_POINTS: CurrencyRepoFields.armoury_points
        }.get(currency)
