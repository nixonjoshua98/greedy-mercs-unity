from __future__ import annotations

import datetime as dt
from typing import Optional

from bson import ObjectId
from pydantic import Field

from src.common.types import QuestID
from src.pymodels import BaseModel
from src.request import ServerRequest

"""
    Merc Quests
        - Reward a merc upon completetion
        - Do not have multiple tiers (once done then thats it)

    Timed Quests (planned to have multiple completion tiers)
        - Daily
        - Weekly

    Achievement Quests (planned to have multiple completion tiers)
        - No time restriction
"""


def get_quests_repo(request: ServerRequest) -> QuestsRepository:
    return QuestsRepository(request.app.state.mongo)


class Fields:
    user_id = "userId"
    quest_id = "questId"
    completed_at = "completedAt"


class MercQuestModel(BaseModel):
    user_id: ObjectId = Field(..., alias=Fields.user_id)
    quest_id: QuestID = Field(..., alias=Fields.quest_id)
    completed_at: dt.datetime = Field(..., alias=Fields.completed_at)


class QuestsRepository:
    def __init__(self, client):
        self.merc_quests = client.database["completedMercQuests"]
        self.timed_quests = client.database["timedQuestProgress"]
        self.achievements = client.database["permanentQuestProgress"]

    # = Merc Quests = #

    async def get_completed_merc_quests(self, uid: ObjectId) -> list[MercQuestModel]:
        ls = await self.merc_quests.find({Fields.user_id: uid}).to_list(length=None)

        return [MercQuestModel.parse_obj(doc) for doc in ls]

    async def get_merc_quest(self, uid: ObjectId, questid: QuestID) -> Optional[MercQuestModel]:
        doc: dict = await self.merc_quests.find_one({Fields.user_id: uid, Fields.quest_id: questid})
        return MercQuestModel.parse_obj(doc) if doc else None

    async def add_merc_quest(self, model: MercQuestModel):
        await self.merc_quests.insert_one(model.dict())
