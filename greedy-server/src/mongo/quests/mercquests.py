from __future__ import annotations

import datetime as dt
from typing import Optional

from bson import ObjectId
from pydantic import Field

from src.common.types import QuestID
from src.models import BaseModel

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
        doc: dict = await self._quests.find_one({Fields.user_id: uid, Fields.quest_id: questid})
        return MercQuestModel.parse_obj(doc) if doc else None

    async def add_quest(self, model: MercQuestModel):
        await self._quests.insert_one(model.dict())
