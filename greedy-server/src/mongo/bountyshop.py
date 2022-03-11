from __future__ import annotations

import datetime as dt

from bson import ObjectId
from pydantic import Field

from src.pymodels import BaseDocument
from src.request import ServerRequest


def bountyshop_repository(request: ServerRequest) -> BountyShopRepository:
    return BountyShopRepository(request.app.state.mongo)


class Fields:
    USER_ID = "userId"
    ITEM_ID = "itemId"
    PURCHASE_TIME = "purchaseTime"
    RESET_TIME = "prevResetTime"
    PURCHASE_COST = "purchaseCost"


class BountyShopPurchaseModel(BaseDocument):
    user_id: ObjectId = Field(..., alias=Fields.USER_ID)
    item_id: str = Field(..., alias=Fields.ITEM_ID)
    purchase_time: dt.datetime = Field(..., alias=Fields.PURCHASE_TIME)
    reset_time: dt.datetime = Field(..., alias=Fields.RESET_TIME)
    purchase_cost: int = Field(..., alias=Fields.PURCHASE_COST)


class BountyShopRepository:
    def __init__(self, client):
        self._purchases_col = client.database["bountyShopPurchases"]

    async def add_purchase(
        self, uid: ObjectId, item_id: str, reset_time: dt.datetime, cost: int
    ):
        await self._purchases_col.insert_one(
            {
                Fields.USER_ID: uid,
                Fields.ITEM_ID: item_id,
                Fields.PURCHASE_TIME: dt.datetime.utcnow(),
                Fields.RESET_TIME: reset_time,
                Fields.PURCHASE_COST: cost,
            }
        )

    async def get_daily_item_purchases(
        self,
        uid: ObjectId,
        item_id: str,
        reset_time: dt.datetime
    ) -> list[BountyShopPurchaseModel]:
        """
        Fetch all daily purchases for a single item
        """
        results = await self._purchases_col.find(
            {
                Fields.USER_ID: uid,
                Fields.ITEM_ID: item_id,
                Fields.RESET_TIME: reset_time,
            }
        ).to_list(length=None)

        return [BountyShopPurchaseModel.parse_obj(r) for r in results]

    async def get_daily_purchases(self, uid: ObjectId, reset_time: dt.datetime) -> list[BountyShopPurchaseModel]:
        results = await self._purchases_col.find(
            {Fields.USER_ID: uid, Fields.RESET_TIME: reset_time}
        ).to_list(length=None)

        return [BountyShopPurchaseModel.parse_obj(r) for r in results]
