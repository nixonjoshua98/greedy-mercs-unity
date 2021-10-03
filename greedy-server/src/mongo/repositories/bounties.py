from __future__ import annotations

from typing import Union
from bson import ObjectId
from pymongo import ReturnDocument

import datetime as dt

from src import logger
from src.routing import ServerRequest
from src.mongo.models import UserBountiesModel

"""
_id (User ID): ObjectId
lastClaimTime: dt.datetime
bounties: [
    {"bountyId": 4, "isActive": False},
    ...
]
"""


def bounties_repository(request: ServerRequest) -> BountiesRepository:
    return BountiesRepository(request.app.state.mongo)


class BountiesRepository:
    def __init__(self, client):
        db = client.get_default_database()

        self._col = db["userBounties"]

    async def get_user(self, uid) -> UserBountiesModel:
        row = await self._find_one(uid)

        return UserBountiesModel(**row)

    async def add_new_bounty(self, uid, bid) -> None:
        user = await self.get_user(uid)

        # Bounty was not loaded, so we assume it is not unlocked
        if bid not in [b.bounty_id for b in user.bounties]:
            await self._col.update_one(
                {"_id": uid}, {"bounties": {
                    "addToSet": {
                        "bountyId": bid,
                        "isActive": False
                    }}})

        else:  # Should never be called assuming outside logic is correct
            logger.warning(f"Attempted to insert a duplicated bounty '{bid}' for user '{uid}'")

    async def set_active_bounties(self, uid, ids: list) -> None:

        # Load the bounties so we can set the other unlocked bounties to 'False' easily
        user = await self.get_user(uid)

        for b in user.bounties:
            is_active = b.bounty_id in ids

            await self._col.update_one(
                {"_id": user.id, "bounties.bountyId": b.bounty_id},
                {"$set": {"bounties.$.isActive": is_active}})

    async def set_claim_time(self, uid: Union[str, ObjectId], claim_time: dt.datetime) -> None:
        await self._update_one(uid, {"$set": {"lastClaimTime": claim_time}})

    # === Internal Methods === #

    async def _find_one(self, uid) -> dict:
        return await self._col.find_one_and_update(
            {"_id": uid}, {
                "$setOnInsert": {
                    "lastClaimTime": dt.datetime.utcnow()
                }
            },
            return_document=ReturnDocument.AFTER,
            upsert=True
        )

    async def _update_one(self, uid, update) -> None:
        await self._col.update_one({"_id": uid}, update, upsert=True)
