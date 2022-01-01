import datetime as dt

from bson import ObjectId

from src.mongo.repositories.bountyshop import BountyShopRepository
from src.resources.bountyshop.models import PurchasableBountyShopItem
from src.routing.handlers.abc import BaseHandler


class BaseBountyShopPurchaseHandler(BaseHandler):
    shop_repo: BountyShopRepository
    prev_reset: dt.datetime

    async def log_purchase(self, uid: ObjectId, item: PurchasableBountyShopItem):
        await self.shop_repo.add_purchase(uid, item.id, self.prev_reset, item.purchase_cost)

    async def get_item_purchase_count(self, uid: ObjectId, item_id: str, prev_reset: dt.datetime) -> int:
        purchases = await self.shop_repo.get_daily_item_purchases(uid, item_id, prev_reset)

        return len(purchases)
