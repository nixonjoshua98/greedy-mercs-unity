from bson import ObjectId
from fastapi import Depends

from src import utils
from src.common.types import QuestActionType, QuestID
from src.context import RequestContext
from src.exceptions import HandlerException
from src.handlers import GetUserDailyStatsHandler, GetUserDailyStatsResponse
from src.repositories.currency import CurrencyRepository
from src.repositories.currency import Fields as CurrencyFieldNames
from src.repositories.currency import get_currency_repository
from src.repositories.quests import (DailyQuestModel, DailyQuestsRepository,
                                     get_daily_quests_repo)
from src.request_models import CompleteDailyQuestRequestModel
from src.shared_models import BaseModel, PlayerStatsModel
from src.static_models.quests import DailyQuest

from .create_quests import CreateQuestsHandler, CreateQuestsResponse


class CompleteDailyQuestResponse(BaseModel):
    diamonds_rewarded: int


class CompleteDailyQuestHandler:
    def __init__(
        self,
        ctx: RequestContext = Depends(),
        daily_stats: GetUserDailyStatsHandler = Depends(),
        create_quests: CreateQuestsHandler = Depends(),
        currencies=Depends(get_currency_repository),
        quests=Depends(get_daily_quests_repo)
    ):
        self.ctx = ctx

        self._create_quests = create_quests

        self._daily_stats: GetUserDailyStatsHandler = daily_stats
        self._currencies: CurrencyRepository = currencies
        self._quests: DailyQuestsRepository = quests

    async def handle(self, uid: ObjectId, model: CompleteDailyQuestRequestModel) -> CompleteDailyQuestResponse:

        # Generate the quests for the user
        user_quests: CreateQuestsResponse = await self._create_quests.handle(uid, self.ctx)

        # Look for the unique aily quest
        quest_data: DailyQuest = utils.get(user_quests.daily_quests, quest_id=model.quest_id)

        if quest_data is None:  # Quest does not exist
            raise HandlerException(400, "Quest not found")

        # Quest has already been completed, and the client is not up-to-date
        if await self._quests.get_quest(uid, model.quest_id, self.ctx.prev_daily_refresh) is not None:
            raise HandlerException(400, "Quest already completed")

        # Fetch the confirmed daily stats, ready to check for quest progress
        daily_stats = await self._daily_stats.handle(uid, self.ctx)

        # Check if the quest has been completed, if it hasnt then return an error
        if not await self.is_quest_completed(quest_data, model.local_daily_stats, daily_stats):
            raise HandlerException(400, "Quest is not completed")

        # Run what we need to after the quest had been confirmed to be cleared
        await self.handle_completed_quest(uid, model.quest_id, quest_data)

        # Return a response back to the caller
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
    async def is_quest_completed(
        cls,
        quest: DailyQuest,
        local_stats: PlayerStatsModel,
        daily_stats: GetUserDailyStatsResponse
    ) -> bool:

        if quest.action_type == QuestActionType.PRESTIGE:
            return daily_stats.total_prestiges >= quest.long_value

        elif quest.action_type == QuestActionType.ENEMIES_DEFEATED:
            return local_stats.total_enemies_defeated >= quest.long_value

        elif quest.action_type == QuestActionType.BOSSES_DEFEATED:
            return local_stats.total_bosses_defeated >= quest.long_value

        elif quest.action_type == QuestActionType.TAPS:
            return local_stats.total_taps >= quest.long_value

        raise HandlerException(500, "Quest type not found")
