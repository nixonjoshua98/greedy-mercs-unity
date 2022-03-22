from __future__ import annotations

import datetime as dt
from typing import Optional

from bson import ObjectId
from pydantic import Field

from src.common.types import QuestID
from src.request import ServerRequest
from src.shared_models import BaseModel


def get_merc_quests_repo(request: ServerRequest):
    return MercQuestsRepository(request.app.state.mongo)


class Fields:
    user_id = "userId"
    quest_id = "questId"
    completed_at = "completedAt"


class MercQuestModel(BaseModel):
    user_id: ObjectId = Field(..., alias=Fields.user_id)
    quest_id: QuestID = Field(..., alias=Fields.quest_id)
    completed_at: dt.datetime = Field(..., alias=Fields.completed_at)


class MercQuestsRepository:
    def __init__(self, client):
        self._quests = client.database["completedMercQuests"]

    async def get_all_quests(self, uid: ObjectId) -> list[MercQuestModel]:
        ls = await self._quests.find({Fields.user_id: uid}).to_list(length=None)

        return [MercQuestModel.parse_obj(doc) for doc in ls]

    async def get_quest(self, uid: ObjectId, questid: QuestID) -> Optional[MercQuestModel]:
        doc = await self._quests.find_one({Fields.user_id: uid, Fields.quest_id: questid})
        return MercQuestModel.parse_obj(doc) if doc else None

    async def add_quest(self, model: MercQuestModel):
        await self._quests.insert_one(model.dict())
