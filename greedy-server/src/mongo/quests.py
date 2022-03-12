from __future__ import annotations

import datetime as dt

from bson import ObjectId
from pydantic import Field

from src.common.types import QuestID, QuestType
from src.pymodels import BaseModel
from src.request import ServerRequest


def get_quests_repo(request: ServerRequest) -> QuestsRepository:
    return QuestsRepository(request.app.state.mongo)


class Fields:
    user_id = "userId"
    quest_id = "questId"
    quest_type = "questType"
    completed_at = "completedAt"


class CompletedQuestModel(BaseModel):
    user_id: ObjectId = Field(..., alias=Fields.user_id)
    quest_id: QuestID = Field(..., alias=Fields.quest_id)
    quest_type: QuestType = Field(..., alias=Fields.quest_type)
    completed_at: dt.datetime = Field(..., alias=Fields.completed_at)


class QuestsRepository:
    def __init__(self, client):
        self.collection = client.database["completedUserQuests"]

    async def get_user_quests(self, uid: ObjectId):
        ...

    async def get_completed_merc_quests(self, uid: ObjectId) -> list[CompletedQuestModel]:
        ls = await self.collection.find(
            {Fields.user_id: uid, Fields.quest_type: QuestType.MERC_QUEST}
        ).to_list(length=None)

        return [CompletedQuestModel.parse_obj(doc) for doc in ls]

    async def persistant_quest_completed(self, uid: ObjectId, questid: QuestID, questtype: QuestType) -> bool:
        doc: dict = await self.collection.find_one(
            {Fields.user_id: uid, Fields.quest_id: questid, Fields.quest_type: questtype}
        )
        return doc is not None

    async def add_completed_quest(self, model: CompletedQuestModel):
        await self.collection.insert_one(model.dict())
