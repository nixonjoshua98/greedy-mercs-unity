from __future__ import annotations

from typing import Union
from bson import ObjectId
from pymongo import ReturnDocument

import datetime as dt

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
    """ Used to inject a repository instance. """
    return BountiesRepository(request.app.state.mongo)


class BountiesRepository:
    def __init__(self, client):
        db = client.get_default_database()

        self._col = db["userBounties"]

    async def get_user(self, uid) -> UserBountiesModel:
        row = await self._find_or_create_one(uid)

        return UserBountiesModel(**row)  # Unpack the query return dict into the model

    async def add_new_bounty(self, uid, bid) -> None:
        _ = await self._find_or_create_one(uid)  # Verify that the user document exists

        # Assumes that the document for the user already exists in the database
        await self._col.update_one(
            {"_id": uid, "bounties.BountyId": {"$ne": bid}},
            {"$push": {
                "bounties": {
                    "bountyId": bid,
                    "isActive": False
                }}})

    async def set_active_bounties(self, uid, ids: list) -> None:

        # Load the bounties so we can set the other unlocked bounties to 'False' easily
        user = await self.get_user(uid)

        for b in user.bounties:
            is_active = b.bounty_id in ids  # We also need to 'disable' the inactive bounties

            await self._col.update_one(
                {"_id": user.id, "bounties.bountyId": b.bounty_id},
                {"$set": {"bounties.$.isActive": is_active}})

    async def set_claim_time(self, uid: Union[str, ObjectId], claim_time: dt.datetime):
        """ Upsert the document field 'lastClaimTime' """
        await self._col.update_one({"_id": uid}, {"$set": {"lastClaimTime": claim_time}}, upsert=True)

    # === Internal Methods === #

    async def _find_or_create_one(self, uid) -> dict:
        """ [Internal] Find or create the root user document
        :param uid: User ID
        :return: Query dict result
        """
        return await self._col.find_one_and_update(
            {"_id": uid}, {
                # Default document fields
                "$setOnInsert": {
                    "lastClaimTime": dt.datetime.utcnow()
                }
            },
            return_document=ReturnDocument.AFTER,
            upsert=True
        )
