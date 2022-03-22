from random import Random

import multipledispatch as md
from bson import ObjectId
from fastapi import Depends

from src.context import AuthenticatedRequestContext, RequestContext
from src.dependencies import get_static_quests
from src.shared_models import BaseModel
from src.static_models.quests import DailyQuest, MercQuest, StaticQuests


class CreateQuestsResponse(BaseModel):
    merc_quests: list[MercQuest]
    daily_quests: list[DailyQuest]


class CreateQuestsHandler:
    def __init__(self, quests=Depends(get_static_quests)):
        self._static_quests: StaticQuests = quests

    @md.dispatch(ObjectId, RequestContext)
    async def handle(self, uid: ObjectId, ctx: RequestContext) -> CreateQuestsResponse:
        return CreateQuestsResponse(
            merc_quests=self._static_quests.merc_quests,
            daily_quests=self._get_daily_quests(uid, ctx),
        )

    @md.dispatch(AuthenticatedRequestContext)
    async def handle(self, ctx: AuthenticatedRequestContext) -> CreateQuestsResponse:
        return await self.handle(ctx.user_id, ctx)

    def _get_daily_quests(self, uid: ObjectId, ctx: RequestContext) -> list[DailyQuest]:
        rnd = Random(x=f"{uid}{ctx.prev_daily_refresh.timestamp()}")

        random_sorted = sorted(self._static_quests.available_daily_quests, key=lambda x: rnd.random())

        return random_sorted[:self._static_quests.daily_quests_per_day]


