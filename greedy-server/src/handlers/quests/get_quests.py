import datetime as dt

import multipledispatch as md
from bson import ObjectId
from fastapi import Depends

from src.auth import AuthenticatedRequestContext, RequestContext
from src.common.types import QuestID
from src.dependencies import get_merc_quests_repo
from src.mongo.quests import (DailyQuestsRepository, MercQuestsRepository,
                              get_daily_quests_repo)
from src.shared_models import BaseModel
from src.static_models.quests import DailyQuest, MercQuest

from .create_quests import CreateQuestsHandler, CreateQuestsResponse


class GetQuestsResponse(BaseModel):
    next_daily_refresh: dt.datetime
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
        completed_daily_quests = await self.daily_quests.get_quests_since(uid, ctx.prev_daily_refresh)
        completed_merc_quests = await self.merc_quests.get_all_quests(uid)

        quests: CreateQuestsResponse = await self._create_quests.handle(uid, ctx)

        return GetQuestsResponse(
            next_daily_refresh=ctx.next_daily_refresh,
            merc_quests=quests.merc_quests,
            daily_quests=quests.daily_quests,
            completed_daily_quests=[q.quest_id for q in completed_daily_quests],
            completed_merc_quests=[q.quest_id for q in completed_merc_quests]
        )

    @md.dispatch(AuthenticatedRequestContext)
    async def handle(self, ctx: AuthenticatedRequestContext) -> GetQuestsResponse:
        return await self.handle(ctx.user_id, ctx)
