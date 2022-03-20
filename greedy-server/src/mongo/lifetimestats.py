from __future__ import annotations

from bson import ObjectId
from pydantic import Field

from src.common.types import NumberType
from src.shared_models import BaseModel


class FieldNames:
    user_id = "userId"
    num_prestiges = "totalPrestiges"
    highest_stage = "highestPrestigeStageReached"


class UserLifetimeStatsModel(BaseModel):
    user_id: ObjectId = Field(..., alias=FieldNames.user_id)
    num_prestiges: int = Field(0, alias=FieldNames.num_prestiges)
    highest_stage: int = Field(0, alias=FieldNames.highest_stage)


class LifetimeStatsRepository:
    def __init__(self, client):
        self._stats = client.database["userLifetimeStats"]

    async def get_user_stats(self, uid: ObjectId) -> UserLifetimeStatsModel:
        r = await self._stats.find_one({FieldNames.user_id: uid})

        return UserLifetimeStatsModel.parse_obj(r or {FieldNames.user_id: uid})

    async def incr(self, uid: ObjectId, field: str, value: NumberType):
        await self._update_one(uid, {"$inc": {field: value}})

    async def max(self, uid: ObjectId, field: str, value: NumberType):
        await self._update_one(uid, {"$max": {field: value}})

    # Internal #

    async def _update_one(self, uid, update: dict):
        await self._stats.update_one({FieldNames.user_id: uid}, update, upsert=True)
