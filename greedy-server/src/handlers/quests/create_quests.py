import multipledispatch as md
from bson import ObjectId
from fastapi import Depends

from src.auth import AuthenticatedRequestContext, RequestContext
from src.dependencies import get_static_quests
from src.shared_models import BaseModel
from src.static_models.quests import DailyQuest, MercQuest, StaticQuests

"""
    Quests will be created via code at some point (not just pulled from files)
"""


class CreateQuestsResponse(BaseModel):
    merc_quests: list[MercQuest]
    daily_quests: list[DailyQuest]


class CreateQuestsHandler:
    def __init__(
        self,
        quests=Depends(get_static_quests),
    ):
        self.quests: StaticQuests = quests

    @md.dispatch(ObjectId, RequestContext)
    async def handle(self, uid: ObjectId, ctx: RequestContext) -> CreateQuestsResponse:
        return CreateQuestsResponse(
            merc_quests=self.quests.merc_quests,
            daily_quests=self.quests.daily_quests,
        )

    @md.dispatch(AuthenticatedRequestContext)
    async def handle(self, ctx: AuthenticatedRequestContext) -> CreateQuestsResponse:
        return await self.handle(ctx.user_id, ctx)
