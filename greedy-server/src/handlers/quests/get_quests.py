import datetime as dt

import multipledispatch as md
from bson import ObjectId
from fastapi import Depends

from src.common.types import QuestID
from src.context import AuthenticatedRequestContext, RequestContext
from src.dependencies import get_merc_quests_repo
from src.repositories.quests import (DailyQuestsRepository,
                                     MercQuestsRepository,
                                     get_daily_quests_repo)
from src.shared_models import BaseModel
from src.static_models.quests import DailyQuest, MercQuest

from .create_quests import CreateQuestsHandler, CreateQuestsResponse


class GetQuestsResponse(BaseModel):
    quests_created_at: dt.datetime
    merc_quests: list[MercQuest]
    daily_quests: list[DailyQuest]
    completed_merc_quests: list[QuestID]
    completed_daily_quests: list[QuestID]


class GetQuestsHandler:
    def __init__(
        self,
        create_quests: CreateQuestsHandler = Depends(),
        merc_quests=Depends(get_merc_quests_repo),
        daily_quests=Depends(get_daily_quests_repo),
    ):
        self._create_quests = create_quests

        self.daily_quests: DailyQuestsRepository = daily_quests
        self.merc_quests: MercQuestsRepository = merc_quests

    @md.dispatch(ObjectId, RequestContext)
    async def handle(self, uid: ObjectId, ctx: RequestContext) -> GetQuestsResponse:

        # Get daily quests progress since the last daily refresh
        completed_daily_quests = await self.daily_quests.get_quests_since(uid, ctx.prev_daily_refresh)

        # Fetch the completed merc quests
        completed_merc_quests = await self.merc_quests.get_all_quests(uid)

        # Generate the quests for the user
        quests: CreateQuestsResponse = await self._create_quests.handle(uid, ctx)

        # Create and return an aggregated response for quest progress and generated quests
        return GetQuestsResponse(
            merc_quests=quests.merc_quests,
            daily_quests=quests.daily_quests,
            quests_created_at=ctx.datetime,
            completed_daily_quests=[q.quest_id for q in completed_daily_quests],
            completed_merc_quests=[q.quest_id for q in completed_merc_quests]
        )

    @md.dispatch(AuthenticatedRequestContext)
    async def handle(self, ctx: AuthenticatedRequestContext) -> GetQuestsResponse:
        return await self.handle(ctx.user_id, ctx)
