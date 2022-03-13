import datetime as dt

from bson import ObjectId
from fastapi import Depends

from src.common.types import MercID, QuestID, QuestType
from src.dependencies import get_static_quests
from src.exceptions import ServerException
from src.mongo.mercs import UnlockedMercsRepository, get_unlocked_mercs_repo
from src.mongo.quests import MercQuestModel, QuestsRepository, get_quests_repo
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

        merc_quest = await self._quests.get_merc_quest(uid, quest_id)
        merc_unlocked = await self._mercs.merc_unlocked(uid, quest_data.reward_merc)

        if merc_quest is not None:
            raise ServerException(400, "Quest already completed")
        elif merc_unlocked:
            raise ServerException(400, "Merc already unlocked")

        model = MercQuestModel(user_id=uid, quest_id=quest_id, completed_at=date)

        await self._quests.add_merc_quest(model)
        await self._mercs.insert_units(uid, [quest_data.reward_merc])

        return CompleteMercQuestResponse(unlocked_merc=quest_data.reward_merc)
