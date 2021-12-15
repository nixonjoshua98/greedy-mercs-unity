from __future__ import annotations

import datetime as dt
from typing import Union

from bson import ObjectId
from pydantic import Field
from pymongo import ReturnDocument

from src.pymodels import BaseDocument, BaseModel
from src.routing import ServerRequest


def bounties_repository(request: ServerRequest) -> BountiesRepository:
    return BountiesRepository(request.app.state.mongo)


class Fields:
    BOUNTY_ID = "bountyId"
    IS_ACTIVE = "isActive"
    LAST_CLAIM_TIME = "lastClaimTime"
    BOUNTIES = "bounties"


class UserBountyModel(BaseModel):
    bounty_id: int = Field(..., alias=Fields.BOUNTY_ID)
    is_active: bool = Field(False, alias=Fields.IS_ACTIVE)


class UserBountiesModel(BaseDocument):
    last_claim_time: dt.datetime = Field(..., alias=Fields.LAST_CLAIM_TIME)
    bounties: list[UserBountyModel] = Field([])

    @property
    def active_bounties(self) -> list[UserBountyModel]:
        return [b for b in self.bounties if b.is_active]

    def client_dict(self):
        return self.dict(exclude={"id"})


class BountiesRepository:
    def __init__(self, client):
        self._col = client.database["userBounties"]

    async def get_user_bounties(self, uid) -> UserBountiesModel:
        row = await self._find_or_create_one(uid)

        return UserBountiesModel.parse_obj(row)

    async def add_new_bounty(self, uid, bid) -> None:
        _ = await self._find_or_create_one(uid)  # Verify that the user document exists

        # Assumes that the document for the user already exists in the database
        # We push the bounty to the list if the bountyId is not already present in the list
        await self._col.update_one(
            {"_id": uid, f"{Fields.BOUNTIES}.{Fields.BOUNTY_ID}": {"$ne": bid}},
            {
                "$addToSet": {  # ... or $push
                    Fields.BOUNTIES: {Fields.BOUNTY_ID: bid, Fields.IS_ACTIVE: False}
                }
            },
        )

    async def update_active_bounties(self, uid, ids: list) -> None:
        user = await self.get_user_bounties(uid)  # Load the user bounties

        for b in user.bounties:
            is_active = (
                b.bounty_id in ids
            )  # We also need to 'disable' the inactive bounties

            await self._col.update_one(
                {"_id": user.id, f"{Fields.BOUNTIES}.{Fields.BOUNTY_ID}": b.bounty_id},
                {"$set": {f"{Fields.BOUNTIES}.$.{Fields.IS_ACTIVE}": is_active}},
            )

    async def set_claim_time(self, uid: Union[str, ObjectId], claim_time: dt.datetime):
        await self._col.update_one(
            {"_id": uid}, {"$set": {Fields.LAST_CLAIM_TIME: claim_time}}, upsert=True
        )

    # === Internal Methods === #

    async def _find_or_create_one(self, uid) -> dict:
        return await self._col.find_one_and_update(
            {"_id": uid},
            {"$setOnInsert": {Fields.LAST_CLAIM_TIME: dt.datetime.utcnow()}},
            return_document=ReturnDocument.AFTER,
            upsert=True,
        )
