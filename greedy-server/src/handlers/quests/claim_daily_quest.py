from bson import ObjectId
from fastapi import Depends

from src.auth import RequestContext
from src.common.types import QuestActionType, QuestID, QuestType
from src.dependencies import get_static_quests
from src.exceptions import HandlerException
from src.handlers import GetUserDailyStatsHandler, GetUserDailyStatsResponse
from src.mongo.currency import CurrencyRepository
from src.mongo.currency import Fields as CurrencyFieldNames
from src.mongo.currency import get_currency_repository
from src.mongo.quests import (DailyQuestModel, DailyQuestsRepository,
                              get_daily_quests_repo)
from src.request_models import CompleteDailyQuestRequestModel
from src.shared_models import BaseModel
from src.static_models.quests import DailyQuest, StaticQuests


class CompleteDailyQuestResponse(BaseModel):
    diamonds_rewarded: int


class CompleteDailyQuestHandler:
    def __init__(
        self,
        ctx: RequestContext = Depends(),
        daily_stats: GetUserDailyStatsHandler = Depends(),
        currencies=Depends(get_currency_repository),
        quests=Depends(get_daily_quests_repo),
        quests_data=Depends(get_static_quests)
    ):
        self.ctx = ctx

        self._daily_stats: GetUserDailyStatsHandler = daily_stats
        self._currencies: CurrencyRepository = currencies
        self._quests: DailyQuestsRepository = quests

        self._quests_data: StaticQuests = quests_data

    async def handle(self, uid: ObjectId, model: CompleteDailyQuestRequestModel) -> CompleteDailyQuestResponse:

        quest_data: DailyQuest = self._quests_data.get_quest(QuestType.DAILY_QUEST, model.quest_id)

        if quest_data is None:
            raise HandlerException(400, "Quest not found")

        if await self._quests.get_quest(uid, model.quest_id, self.ctx.prev_daily_refresh) is not None:
            raise HandlerException(400, "Quest already completed")

        daily_stats = await self._daily_stats.handle(
            uid,
            self.ctx.prev_daily_refresh,
            self.ctx.next_daily_refresh
        )

        if not await self.is_quest_completed(quest_data, daily_stats):
            raise HandlerException(400, "Quest is not completed")

        await self.handle_completed_quest(uid, model.quest_id, quest_data)

        return CompleteDailyQuestResponse(
            diamonds_rewarded=quest_data.diamonds_rewarded
        )

    async def handle_completed_quest(self, uid: ObjectId, quest_id: QuestID, quest: DailyQuest):
        await self._quests.add_quest(DailyQuestModel(
            user_id=uid,
            quest_id=quest_id,
            completed_at=self.ctx.datetime
        ))

        await self._currencies.incr(uid, CurrencyFieldNames.diamonds, quest.diamonds_rewarded)

    @classmethod
    async def is_quest_completed(cls, quest: DailyQuest, dailystats: GetUserDailyStatsResponse) -> bool:
        if quest.action_type == QuestActionType.PRESTIGE:
            return dailystats.total_prestiges >= quest.num_prestiges

        raise Exception()
