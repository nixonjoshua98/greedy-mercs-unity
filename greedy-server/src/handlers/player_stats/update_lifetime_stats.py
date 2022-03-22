
from bson import ObjectId
from fastapi import Depends

from src.repositories.lifetimestats import (LifetimeStatsRepository,
                                            get_lifetime_stats_repo)
from src.shared_models import BaseModel, PlayerStatsModel


class UpdateLifetimeStatsResponse(BaseModel):
    lifetime_stats: PlayerStatsModel


class UpdateLifetimeStatsHandler:
    def __init__(self, lifetime_stats_repo=Depends(get_lifetime_stats_repo)):
        self._lifetime_stats: LifetimeStatsRepository = lifetime_stats_repo

    async def handle(self, user_id: ObjectId, model: PlayerStatsModel):
        lifetime: PlayerStatsModel = await self._lifetime_stats.update(user_id, model)

        return UpdateLifetimeStatsResponse(lifetime_stats=lifetime)
