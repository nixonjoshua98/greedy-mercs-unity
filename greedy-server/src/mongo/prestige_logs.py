from __future__ import annotations

import datetime as dt

from bson import ObjectId
from pydantic import Field
from pymongo import ReturnDocument

from src.classes import DateRange
from src.common.types import NumberType
from src.models import BaseDocument, BaseModel
from src.request import ServerRequest


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

    async def count_prestiges_between(self, uid: ObjectId, range_: DateRange) -> int:
        return await self._logs.count_documents(
            {FieldNames.user_id: uid, FieldNames.date: {"$gt": range_.from_, "$lt": range_.to_}}
        )

    async def insert_prestige_log(self, model: PrestigeLogModel):
        await self._logs.insert_one(model.dict())
