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


class UserBountyModel(BaseModel):

    class Aliases:
        user_id = "userId"
        bounty_id = "bountyId"
        is_active = "isActive"

    user_id: ObjectId = Field(..., alias=Aliases.user_id)
    bounty_id: BountyID = Field(..., alias=Aliases.bounty_id)
    is_active: bool = Field(False, alias=Aliases.is_active)


class UserBountyMetaData(BaseModel):

    class Aliases:
        user_id = "userId"
        last_claim_time = "lastClaimTime"

    user_id: ObjectId = Field(..., alias=Aliases.user_id)
    last_claim_time: dt.datetime = Field(..., alias=Aliases.last_claim_time)


class UserBountiesDataModel(BaseModel):
    user_id: ObjectId
    last_claim_time: dt.datetime
    bounties: list[UserBountyModel]

    @property
    def active_bounties(self) -> list[UserBountyModel]:
        return [b for b in self.bounties if b.is_active]


class BountiesRepository:
    def __init__(self, client):
        self.metadata = client.database["bountyMetadata"]
        self.bounties = client.database["unlockedBounties"]

    async def get_user_bounties(self, uid: ObjectId) -> UserBountiesDataModel:
        meta: UserBountyMetaData = await self._find_metadata(uid)
        bounties: list[UserBountyModel] = await self._find_bounties(uid)

        return UserBountiesDataModel(
            user_id=uid,
            last_claim_time=meta.last_claim_time,
            bounties=bounties
        )

    async def insert_new_bounties(self, uid: ObjectId, bounty_ids: list[int]):
        requests = []

        for bounty_id in bounty_ids:
            r = InsertOne({
                UserBountyModel.Aliases.user_id: uid,
                UserBountyModel.Aliases.bounty_id: bounty_id,
                UserBountyModel.Aliases.is_active: False
            })
            requests.append(r)

        if requests: await self.bounties.bulk_write(requests)

    async def update_active_bounties(self, uid, ids: list) -> None:
        requests = [
            UpdateMany({UserBountyModel.Aliases.user_id: uid}, {"$set": {UserBountyModel.Aliases.is_active: False}})
        ]

        for bounty_id in ids:
            r = UpdateOne(self._unique_bounty(uid, bounty_id), {"$set": {UserBountyModel.Aliases.is_active: True}})

            requests.append(r)

        await self.bounties.bulk_write(requests)

    async def set_claim_time(self, uid: Union[str, ObjectId], claim_time: dt.datetime):
        await self.metadata.update_one(
            {UserBountyMetaData.Aliases.user_id: uid},
            {"$set": {UserBountyMetaData.Aliases.last_claim_time: claim_time}},
            upsert=True
        )

    async def _find_metadata(self, uid: ObjectId) -> UserBountyMetaData:
        doc = await self.metadata.find_one_and_update(
            {UserBountyMetaData.Aliases.user_id: uid},
            {"$setOnInsert": {UserBountyMetaData.Aliases.last_claim_time: dt.datetime.utcnow()}},
            return_document=ReturnDocument.AFTER, upsert=True,
        )
        return UserBountyMetaData.parse_obj(doc)

    async def _find_bounties(self, uid: ObjectId) -> list[UserBountyModel]:
        ls: list[dict] = await self.bounties.find({UserBountyModel.Aliases.user_id: uid}).to_list(length=None)
        return [UserBountyModel.parse_obj(doc) for doc in ls]

    # = Indexes =#

    @staticmethod
    def _unique_bounty(uid: ObjectId, bid: int):
        return {UserBountyModel.Aliases.user_id: uid, UserBountyModel.Aliases.bounty_id: bid}
