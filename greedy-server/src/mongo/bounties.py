from __future__ import annotations

import datetime as dt
from typing import Union

from bson import ObjectId
from pydantic import Field
from pymongo import InsertOne, ReturnDocument, UpdateMany, UpdateOne

from src.pymodels import BaseModel
from src.request import ServerRequest
from src.static_models.bounties import BountyID


def bounties_repository(request: ServerRequest) -> BountiesRepository:
    return BountiesRepository(request.app.state.mongo)


class FieldNames:
    user_id = "userId"
    bounty_id = "bountyId"
    is_active = "isActive"
    last_claim_time = "lastClaimTime"
    bounties = "bounties"


class UserBountyModel(BaseModel):
    user_id: ObjectId = Field(..., alias=FieldNames.user_id)
    bounty_id: BountyID = Field(..., alias=FieldNames.bounty_id)
    is_active: bool = Field(False, alias=FieldNames.is_active)


class UserBountiesDataModel(BaseModel):
    user_id: ObjectId = Field(..., alias=FieldNames.user_id)
    last_claim_time: dt.datetime = Field(..., alias=FieldNames.last_claim_time)
    bounties: list[UserBountyModel] = Field(..., alias=FieldNames.bounties)

    @property
    def active_bounties(self) -> list[UserBountyModel]:
        return [b for b in self.bounties if b.is_active]


class BountiesRepository:
    def __init__(self, client):
        self.metadata = client.database["bountyMetadata"]
        self.bounties = client.database["unlockedBounties"]

    async def get_user_bounties(self, uid: ObjectId) -> UserBountiesDataModel:
        meta = await self._find_metadata(uid)
        bounties = await self._find_bounties(uid)

        return UserBountiesDataModel.parse_obj({**meta, FieldNames.bounties: bounties})

    async def insert_new_bounties(self, uid: ObjectId, bounty_ids: list[int]):
        requests = []

        for bounty_id in bounty_ids:
            r = InsertOne({
                FieldNames.user_id: uid,
                FieldNames.bounty_id: bounty_id,
                FieldNames.is_active: False
            })
            requests.append(r)

        await self.bounties.bulk_write(requests)

    async def update_active_bounties(self, uid, ids: list) -> None:
        requests = [UpdateMany({FieldNames.user_id: uid}, {"$set": {FieldNames.is_active: False}})]

        for bounty_id in ids:
            r = UpdateOne(self._unique_bounty(uid, bounty_id), {"$set": {FieldNames.is_active: True}})

            requests.append(r)

        await self.bounties.bulk_write(requests)

    async def set_claim_time(self, uid: Union[str, ObjectId], claim_time: dt.datetime):
        await self.metadata.update_one({FieldNames.user_id: uid}, {"$set": {FieldNames.last_claim_time: claim_time}})

    async def _find_metadata(self, uid: ObjectId) -> dict:
        return await self.metadata.find_one_and_update(
            {FieldNames.user_id: uid},
            {"$setOnInsert": {FieldNames.last_claim_time: dt.datetime.utcnow()}},
            return_document=ReturnDocument.AFTER,
            upsert=True,
        )

    async def _find_bounties(self, uid: ObjectId) -> list[dict]:
        return await self.bounties.find({FieldNames.user_id: uid}).to_list(length=None)

    # = Indexes =#

    @staticmethod
    def _unique_bounty(uid: ObjectId, bid: int):
        return {FieldNames.user_id: uid, FieldNames.bounty_id: bid}
