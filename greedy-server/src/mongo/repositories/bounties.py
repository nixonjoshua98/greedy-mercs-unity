from __future__ import annotations

from typing import Union
from bson import ObjectId
from pymongo import ReturnDocument

from src.routing import ServerRequest

import datetime as dt

from src.mongo.models import UserBountiesModel


def bounties_repository(request: ServerRequest) -> BountiesRepository:
    return BountiesRepository(request.app.state.mongo)


class BountiesRepository:
    def __init__(self, client):
        self._collection = client.get_default_database()["userBountiesData"]

    async def get_user(self, uid):
        row = await self._find_one(uid)

        return UserBountiesModel(**row)

    async def add_new_bounty(self, uid, bid):
        await self._collection.update_one(
            {"_id": uid, "bounties": {"$not": {"$elemMatch": {"bountyId": bid}}}},
            {
                "$addToSet": {
                    "bounties": {
                        "bountyId": bid,
                        "isActive": False
                    }
                }},
            upsert=True)

    async def set_active_bounties(self, uid, ids: list):
        result = await self._find_one(uid)

        for b in result.get("bounties", []):
            is_active = b["bountyId"] in ids

            await self._collection.update_one(
                {"_id": result["_id"], "bounties.bountyId": b["bountyId"]},
                {"$set": {"bounties.$.isActive": is_active}})

    async def set_claim_time(self, uid: Union[str, ObjectId], claim_time: dt.datetime) -> None:
        await self._update_one(uid, {"$set": {"lastClaimTime": claim_time}})

    async def _find_one(self, uid):
        return await self._collection.find_one_and_update(
            {"_id": uid}, {
                "$setOnInsert": {
                    "lastClaimTime": dt.datetime.utcnow()
                }
            },
            return_document=ReturnDocument.AFTER,
            upsert=True
        )

    async def _update_one(self, uid, update):
        return await self._collection.update_one({"_id": uid}, update, upsert=True)
