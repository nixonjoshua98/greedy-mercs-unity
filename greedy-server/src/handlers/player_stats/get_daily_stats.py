
import datetime as dt

import multipledispatch as md
from bson import ObjectId
from fastapi import Depends
from pydantic import Field

from src.context import AuthenticatedRequestContext, RequestContext
from src.mongo.prestigelogs import (PrestigeLogsRepository,
                                    get_prestige_logs_repo)
from src.shared_models import BaseModel


class GetUserDailyStatsResponse(BaseModel):
    next_refresh: dt.datetime

    total_prestiges: int = Field(0, alias="totalPrestiges")


class GetUserDailyStatsHandler:
    def __init__(self, prestige_logs=Depends(get_prestige_logs_repo)):
        self._prestige_logs: PrestigeLogsRepository = prestige_logs

    @md.dispatch(ObjectId, dt.datetime, dt.datetime)
    async def handle(
            self,
            user_id: ObjectId,
            from_date: dt.datetime,
            to_date: dt.datetime
    ):
        return GetUserDailyStatsResponse(
            next_refresh=to_date,
            total_prestiges=await self._prestige_logs.count_prestiges_between(user_id, from_date, to_date)
        )

    @md.dispatch(ObjectId, RequestContext)
    async def handle(self, uid: ObjectId, ctx: RequestContext):
        return await self.handle(uid, ctx.prev_daily_refresh, ctx.next_daily_refresh)

    @md.dispatch(AuthenticatedRequestContext)
    async def handle(self, ctx: AuthenticatedRequestContext):
        return await self.handle(ctx.user_id, ctx.prev_daily_refresh, ctx.next_daily_refresh)
