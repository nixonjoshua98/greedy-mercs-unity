from __future__ import annotations

import datetime as dt

from bson import ObjectId
from pydantic import Field
import multipledispatch as md
from src.pymodels import BaseModel
from src.routing import ServerRequest


def bountyshop_repository(request: ServerRequest) -> BountyShopRepository:
    return BountyShopRepository(request.app.state.mongo)


class BountyShopPurchaseModel(BaseModel):

    class Aliases:
        USER_ID = "userId"
        ITEM_ID = "itemId"
        PURCHASE_TIME = "purchaseTime"
        RESET_TIME = "prevResetTime"
        PURCHASE_COST = "purchaseCost"

    user_id: ObjectId = Field(..., alias=Aliases.USER_ID)
    item_id: str = Field(..., alias=Aliases.ITEM_ID)
    purchase_time: dt.datetime = Field(..., alias=Aliases.PURCHASE_TIME)
    purchase_cost: int = Field(..., alias=Aliases.PURCHASE_COST)

    def client_dict(self):
        return self.dict(exclude={"user_id", "reset_time", "purchase_time"})


class BountyShopRepository:
    def __init__(self, client):
        self.purchases = client.database["bountyShopPurchases"]

    async def add_purchase(self, uid: ObjectId, item_id: str, reset_time: dt.datetime, cost: int):
        """
        Insert a purchase log into the database
        :param uid: User id
        :param item_id: Item id
        :param reset_time: Previous reset datetime
        :param cost: Purchase cost
        """
        await self.purchases.insert_one({
            BountyShopPurchaseModel.Aliases.USER_ID: uid,
            BountyShopPurchaseModel.Aliases.ITEM_ID: item_id,
            BountyShopPurchaseModel.Aliases.PURCHASE_TIME: dt.datetime.utcnow(),
            BountyShopPurchaseModel.Aliases.RESET_TIME: reset_time,
            BountyShopPurchaseModel.Aliases.PURCHASE_COST: cost,
        })

    @md.dispatch(ObjectId, str, dt.datetime)
    async def daily_purchases(
        self,
        uid: ObjectId,
        item_id: str,
        reset_time: dt.datetime
    ) -> list[BountyShopPurchaseModel]:
        """
        Fetch all purchases made by a user for a given daily reset for a single item
        :param uid: User id
        :param item_id: item id
        :param reset_time: The previous daily reset datetime
        :return:
            List of bounty shop purchases
        """
        results = await self.purchases.find({
            BountyShopPurchaseModel.Aliases.USER_ID: uid,
            BountyShopPurchaseModel.Aliases.ITEM_ID: item_id,
            BountyShopPurchaseModel.Aliases.RESET_TIME: reset_time,
        }).to_list(length=None)

        return [BountyShopPurchaseModel.parse_obj(r) for r in results]

    @md.dispatch(ObjectId, dt.datetime)
    async def daily_purchases(self, uid: ObjectId, reset_time: dt.datetime) -> list[BountyShopPurchaseModel]:
        """
        Fetch all purchases (include all items) made by a user since the previous reset
        :param uid: User id
        :param reset_time: Previous reset datetime
        :return:
            List of bounty shop purchases
        """
        results = await self.purchases.find({
            BountyShopPurchaseModel.Aliases.USER_ID: uid,
            BountyShopPurchaseModel.Aliases.RESET_TIME: reset_time
        }).to_list(length=None)

        return [BountyShopPurchaseModel.parse_obj(r) for r in results]
