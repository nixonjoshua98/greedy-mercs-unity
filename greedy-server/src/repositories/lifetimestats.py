from __future__ import annotations

from bson import ObjectId
from pymongo import ReturnDocument

from src.common.types import NumberType
from src.request import ServerRequest
from src.shared_models import PlayerStatsFields, PlayerStatsModel


def get_lifetime_stats_repo(request: ServerRequest):
    return LifetimeStatsRepository(request.app.state.mongo)


class FieldNames:
    user_id = "userId"


class LifetimeStatsRepository:
    def __init__(self, client):
        self._stats = client.database["userLifetimeStats"]

    async def update(self, uid: ObjectId, model: PlayerStatsModel) -> PlayerStatsModel:
        update_ = {
            "$inc": {
                PlayerStatsFields.total_enemies_defeated: model.total_enemies_defeated,
                PlayerStatsFields.total_bosses_defeated: model.total_bosses_defeated,
                PlayerStatsFields.total_taps: model.total_taps
            }
        }
        return await self._find_one_and_update(uid, update_)

    async def incr(self, uid: ObjectId, field: str, value: NumberType):
        await self._update_one(uid, {"$inc": {field: value}})

    async def max(self, uid: ObjectId, field: str, value: NumberType):
        await self._update_one(uid, {"$max": {field: value}})

    # Internal #

    async def _update_one(self, uid, update: dict):
        await self._stats.update_one({FieldNames.user_id: uid}, update, upsert=True)

    async def _find_one_and_update(self, uid, update: dict) -> PlayerStatsModel:
        r = await self._stats.find_one_and_update(
            {FieldNames.user_id: uid},
            update,
            upsert=True,
            return_document=ReturnDocument.AFTER
        )
        return PlayerStatsModel.parse_obj(r)
