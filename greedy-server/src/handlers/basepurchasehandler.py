import datetime as dt

from bson import ObjectId

from src.exceptions import HandlerException
from src.mongo.bountyshop import BountyShopRepository
from src.models import BaseModel
from src.static_models.bountyshop.models import PurchasableBountyShopItem


class BaseBountyShopPurchaseHandler:
    shop_repo: BountyShopRepository
    prev_reset: dt.datetime

    async def log_purchase(self, uid: ObjectId, item: PurchasableBountyShopItem):
        await self.shop_repo.add_purchase(uid, item.id, self.prev_reset, item.purchase_cost)

    async def get_item_purchase_count(self, uid: ObjectId, item_id: str, prev_reset: dt.datetime) -> int:
        purchases = await self.shop_repo.get_daily_item_purchases(uid, item_id, prev_reset)

        return len(purchases)
