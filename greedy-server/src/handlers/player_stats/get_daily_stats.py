
import datetime as dt

import multipledispatch as md
from bson import ObjectId
from fastapi import Depends

from src.context import AuthenticatedRequestContext, RequestContext
from src.repositories.prestigelogs import (PrestigeLogsRepository,
                                           get_prestige_logs_repo)
from src.shared_models import PlayerStatsModel


class GetUserDailyStatsResponse(PlayerStatsModel):
    created_time: dt.datetime


class GetUserDailyStatsHandler:
    def __init__(self, prestige_logs=Depends(get_prestige_logs_repo)):
        self._prestige_logs: PrestigeLogsRepository = prestige_logs

    @md.dispatch(ObjectId, RequestContext)
    async def handle(self, uid: ObjectId, ctx: RequestContext):
        prestiges = await self._prestige_logs.count_user_prestiges(uid, ctx.prev_daily_refresh, ctx.next_daily_refresh)

        return GetUserDailyStatsResponse(
            created_time=ctx.datetime,
            total_prestiges=prestiges
        )

    @md.dispatch(AuthenticatedRequestContext)
    async def handle(self, ctx: AuthenticatedRequestContext):
        return await self.handle(ctx.user_id, ctx)
