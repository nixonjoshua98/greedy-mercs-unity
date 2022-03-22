from __future__ import annotations

import datetime as dt
from typing import Optional

from bson import ObjectId
from pydantic import Field

from src.common.types import QuestID
from src.request import ServerRequest
from src.shared_models import BaseModel


def get_daily_quests_repo(request: ServerRequest):
    return DailyQuestsRepository(request.app.state.mongo)


class Fields:
    user_id = "userId"
    quest_id = "questId"
    completed_at = "completedAt"


class DailyQuestModel(BaseModel):
    user_id: ObjectId = Field(..., alias=Fields.user_id)
    quest_id: QuestID = Field(..., alias=Fields.quest_id)
    completed_at: dt.datetime = Field(..., alias=Fields.completed_at)


class DailyQuestsRepository:
    def __init__(self, client):
        self._quests = client.database["completedDailyQuests"]

    async def get_quests_since(self, uid: ObjectId, last_refresh: dt.datetime) -> list[DailyQuestModel]:
        ls = await self._quests.find(
            {Fields.user_id: uid, Fields.completed_at: {"$gt": last_refresh}}
        ).to_list(length=None)

        return [DailyQuestModel.parse_obj(doc) for doc in ls]

    async def get_quest(self, uid: ObjectId, questid: QuestID, last_refresh: dt.datetime) -> Optional[DailyQuestModel]:
        doc = await self._quests.find_one(
            {Fields.user_id: uid, Fields.quest_id: questid, Fields.completed_at: {"$gt": last_refresh}}
        )
        return DailyQuestModel.parse_obj(doc) if doc else None

    async def add_quest(self, model: DailyQuestModel):
        await self._quests.insert_one(model.dict())
