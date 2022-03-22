
import multipledispatch as md
from bson import ObjectId
from fastapi import Depends

from src.context import AuthenticatedRequestContext
from src.repositories.prestigelogs import (PrestigeLogsRepository,
                                           get_prestige_logs_repo)
from src.shared_models import PlayerStatsModel


class GetLifetimeStatsResponse(PlayerStatsModel):
    ...


class GetLifetimeStatsHandler:
    def __init__(self, prestige_logs=Depends(get_prestige_logs_repo)):
        self._prestige_logs: PrestigeLogsRepository = prestige_logs

    @md.dispatch(ObjectId)
    async def handle(self, user_id: ObjectId):
        return GetLifetimeStatsResponse(
            total_prestiges=await self._prestige_logs.count_user_prestiges(user_id),
            highest_prestige_stage=await self._prestige_logs.highest_prestige_stage(user_id)

        )

    @md.dispatch(AuthenticatedRequestContext)
    async def handle(self, ctx: AuthenticatedRequestContext):
        return await self.handle(ctx.user_id)
