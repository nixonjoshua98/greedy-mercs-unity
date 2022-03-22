from __future__ import annotations

from bson import ObjectId

from src.common.types import NumberType


class FieldNames:
    user_id = "userId"


class LifetimeStatsRepository:
    def __init__(self, client):
        self._stats = client.database["userLifetimeStats"]

    async def incr(self, uid: ObjectId, field: str, value: NumberType):
        await self._update_one(uid, {"$inc": {field: value}})

    async def max(self, uid: ObjectId, field: str, value: NumberType):
        await self._update_one(uid, {"$max": {field: value}})

    # Internal #

    async def _update_one(self, uid, update: dict):
        await self._stats.update_one({FieldNames.user_id: uid}, update, upsert=True)
