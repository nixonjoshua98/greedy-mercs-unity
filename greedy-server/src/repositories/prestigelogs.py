from __future__ import annotations

import datetime as dt

import multipledispatch as md
from bson import ObjectId
from pydantic import Field

from src.request import ServerRequest
from src.shared_models import BaseModel


def get_prestige_logs_repo(request: ServerRequest):
    return PrestigeLogsRepository(request.app.state.mongo)


class FieldNames:
    user_id = "userId"
    date = "prestigeDate"
    stage = "prestigeStage"


class PrestigeLogModel(BaseModel):
    user_id: ObjectId = Field(..., alias=FieldNames.user_id)
    date: dt.datetime = Field(..., alias=FieldNames.date)
    stage: int = Field(..., alias=FieldNames.stage)


class PrestigeLogsRepository:
    def __init__(self, client):
        self._logs = client.database["userPrestigeLogs"]

    @md.dispatch(ObjectId)
    async def count_user_prestiges(self, uid: ObjectId) -> int:
        return await self._logs.count_documents({FieldNames.user_id: uid})

    @md.dispatch(ObjectId, dt.datetime, dt.datetime)
    async def count_user_prestiges(self, uid: ObjectId, from_date: dt.datetime, to_date: dt.datetime) -> int:
        return await self._logs.count_documents(
            {FieldNames.user_id: uid, FieldNames.date: {"$gt": from_date, "$lt": to_date}}
        )

    async def highest_prestige_stage(self, uid: ObjectId) -> int:
        doc = await self._logs.find_one({FieldNames.user_id: uid}, sort=[(FieldNames.stage, 1)])
        return (doc or {}).get(FieldNames.stage, 0)

    async def insert_prestige_log(self, model: PrestigeLogModel):
        await self._logs.insert_one(model.dict())
