from __future__ import annotations

import datetime as dt

from bson import ObjectId
from pydantic import Field

from src.common.types import MercID
from src.request import ServerRequest
from src.shared_models import BaseModel


def get_unlocked_mercs_repo(request: ServerRequest) -> UnlockedMercsRepository:
    return UnlockedMercsRepository(request.app.state.mongo)


class Fields:
    user_id = "userId"
    merc_id = "mercId"
    unlocked_at = "unlockedAt"


class UnlockedMercModel(BaseModel):
    user_id: ObjectId = Field(..., alias=Fields.user_id)
    merc_id: MercID = Field(..., alias=Fields.merc_id)
    unlocked_at: dt.datetime = Field(..., alias=Fields.unlocked_at)


class UnlockedMercsRepository:
    def __init__(self, client):
        self._mercs = client.database["userUnlockedMercs"]

    async def merc_unlocked(self, uid: ObjectId, mercid: MercID) -> bool:
        doc: dict = await self._mercs.find_one({Fields.user_id: uid, Fields.merc_id: mercid})
        return doc is not None

    async def insert_units(self, uid: ObjectId, unit_ids: list[int]):
        unlocked_ids: list[int] = [u.merc_id for u in await self.get_user_mercs(uid)]

        for unit_id in (id_ for id_ in unit_ids if id_ not in unlocked_ids):
            await self._mercs.update_one(
                {Fields.user_id: uid, Fields.merc_id: unit_id},
                {"$setOnInsert": {Fields.unlocked_at: dt.datetime.utcnow()}},
                upsert=True
            )

    async def get_user_mercs(self, uid: ObjectId) -> list[UnlockedMercModel]:
        ls: list[dict] = await self._mercs.find({Fields.user_id: uid}).to_list(length=None)

        return [UnlockedMercModel.parse_obj(ele) for ele in ls]
