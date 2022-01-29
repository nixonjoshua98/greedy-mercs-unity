from __future__ import annotations

import datetime as dt

from bson import ObjectId
from pydantic import Field

from src.pymodels import BaseDocument
from src.routing import ServerRequest


def bountyshop_repository(request: ServerRequest) -> BountyShopRepository:
    return BountyShopRepository(request.app.state.mongo)


class BountyShopPurchaseModel(BaseDocument):
    class Aliases:
        USER_ID = "userId"
        ITEM_ID = "itemId"
        PURCHASE_TIME = "purchaseTime"
        RESET_TIME = "prevResetTime"
        PURCHASE_COST = "purchaseCost"

    user_id: ObjectId = Field(..., alias=Aliases.USER_ID)
    item_id: str = Field(..., alias=Aliases.ITEM_ID)
    purchase_time: dt.datetime = Field(..., alias=Aliases.PURCHASE_TIME)
    reset_time: dt.datetime = Field(..., alias=Aliases.RESET_TIME)
    purchase_cost: int = Field(..., alias=Aliases.PURCHASE_COST)

    def client_dict(self):
        return self.dict(exclude={"id", "user_id", "reset_time", "purchase_time"})


class BountyShopRepository:
    def __init__(self, client):
        self._purchases = client.database["bountyShopPurchases"]

    async def add_purchase(
        self, uid: ObjectId, item_id: str, reset_time: dt.datetime, cost: int
    ):
        await self._purchases.insert_one({
            BountyShopPurchaseModel.Aliases.USER_ID: uid,
            BountyShopPurchaseModel.Aliases.ITEM_ID: item_id,
            BountyShopPurchaseModel.Aliases.PURCHASE_TIME: dt.datetime.utcnow(),
            BountyShopPurchaseModel.Aliases.RESET_TIME: reset_time,
            BountyShopPurchaseModel.Aliases.PURCHASE_COST: cost,
        })

    async def get_daily_item_purchases(
        self,
        uid: ObjectId,
        item_id: str,
        reset_time: dt.datetime
    ) -> list[BountyShopPurchaseModel]:
        """Fetch all daily purchases for a single item"""
        results = await self._purchases.find({
            BountyShopPurchaseModel.Aliases.USER_ID: uid,
            BountyShopPurchaseModel.Aliases.ITEM_ID: item_id,
            BountyShopPurchaseModel.Aliases.RESET_TIME: reset_time,
        }).to_list(length=None)

        return [BountyShopPurchaseModel.parse_obj(r) for r in results]

    async def get_daily_purchases(self, uid: ObjectId, reset_time: dt.datetime) -> list[BountyShopPurchaseModel]:
        results = await self._purchases.find({
            BountyShopPurchaseModel.Aliases.USER_ID: uid,
            BountyShopPurchaseModel.Aliases.RESET_TIME: reset_time
        }).to_list(length=None)

        return [BountyShopPurchaseModel.parse_obj(r) for r in results]
