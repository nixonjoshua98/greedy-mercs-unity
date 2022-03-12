import datetime as dt

from bson import ObjectId
from fastapi import Depends

from src.common.types import MercID, QuestID, QuestType
from src.dependencies import get_static_quests
from src.exceptions import ServerException
from src.mongo.mercs import UnlockedMercsRepository, get_unlocked_mercs_repo
from src.mongo.quests import (CompletedQuestModel, QuestsRepository,
                              get_quests_repo)
from src.pymodels import BaseModel
from src.static_models.quests import MercQuest, StaticQuests


class CompleteMercQuestResponse(BaseModel):
    unlocked_merc: MercID


class CompleteMercQuestHandler:
    def __init__(
        self,
        quests=Depends(get_quests_repo),
        mercs=Depends(get_unlocked_mercs_repo),
        quests_data=Depends(get_static_quests)
    ):
        self._quests: QuestsRepository = quests
        self._mercs: UnlockedMercsRepository = mercs

        self._quests_data: StaticQuests = quests_data

    async def handle(
        self,
        uid: ObjectId,
        date: dt.datetime,
        quest_id: QuestID
    ) -> CompleteMercQuestResponse:

        quest_data: MercQuest = self._quests_data.get_quest(QuestType.MERC_QUEST, quest_id)

        if quest_data is None:
            raise ServerException(500, "Quest not found")

        quest_completed = await self._quests.persistant_quest_completed(uid, quest_id, QuestType.MERC_QUEST)
        merc_unlocked = await self._mercs.merc_unlocked(uid, quest_data.reward_merc)

        if quest_completed:
            raise ServerException(400, "Quest already completed")
        elif merc_unlocked:
            raise ServerException(400, "Merc already unlocked")

        model = CompletedQuestModel(
            user_id=uid,
            quest_id=quest_id,
            completed_at=date,
            quest_type=QuestType.MERC_QUEST
        )

        await self._quests.add_completed_quest(model)
        await self._mercs.insert_units(uid, [quest_data.reward_merc])

        return CompleteMercQuestResponse(unlocked_merc=quest_data.reward_merc)
