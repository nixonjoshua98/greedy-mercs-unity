from bson import ObjectId
from fastapi import Depends

from src.auth import RequestContext
from src.common.types import MercID, QuestType
from src.dependencies import get_merc_quests_repo, get_static_quests
from src.exceptions import HandlerException
from src.mongo.mercs import UnlockedMercsRepository, get_unlocked_mercs_repo
from src.mongo.quests import MercQuestModel, MercQuestsRepository
from src.request_models import CompleteMercQuestRequestModel
from src.shared_models import BaseModel
from src.static_models.quests import MercQuest, StaticQuests


class CompleteMercQuestResponse(BaseModel):
    unlocked_merc: MercID


class CompleteMercQuestHandler:
    def __init__(
        self,
        ctx: RequestContext = Depends(),
        quests=Depends(get_merc_quests_repo),
        mercs=Depends(get_unlocked_mercs_repo),
        quests_data=Depends(get_static_quests)
    ):
        self.ctx = ctx

        self._quests: MercQuestsRepository = quests
        self._mercs: UnlockedMercsRepository = mercs

        self._quests_data: StaticQuests = quests_data

    async def handle(self, uid: ObjectId, model: CompleteMercQuestRequestModel) -> CompleteMercQuestResponse:

        quest_data: MercQuest = self._quests_data.get_quest(QuestType.MERC_QUEST, model.quest_id)

        if quest_data is None:
            raise HandlerException(500, "Quest not found")

        merc_quest = await self._quests.get_quest(uid, model.quest_id)
        merc_unlocked = await self._mercs.merc_unlocked(uid, quest_data.reward_merc)

        if merc_quest is not None:
            raise HandlerException(400, "Quest already completed")
        elif merc_unlocked:
            raise HandlerException(400, "Merc already unlocked")
        elif quest_data.required_stage < quest_data.required_stage:
            raise HandlerException(400, "Unlock conditions not met")

        await self.handle_completed_quest(uid, quest_data)

        return CompleteMercQuestResponse(unlocked_merc=quest_data.reward_merc)

    async def handle_completed_quest(self, uid: ObjectId, quest: MercQuest):
        await self._quests.add_quest(MercQuestModel(
            user_id=uid,
            quest_id=quest.quest_id,
            completed_at=self.ctx.datetime
        ))

        await self._mercs.insert_units(uid, [quest.reward_merc])

