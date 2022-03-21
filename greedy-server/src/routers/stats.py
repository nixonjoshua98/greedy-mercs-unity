from fastapi import Depends

from src.auth.handler import (AuthenticatedRequestContext,
                              get_authenticated_context)
from src.dependencies import get_lifetime_stats_repo
from src.handlers import GetUserDailyStatsHandler
from src.mongo.lifetimestats import LifetimeStatsRepository
from src.response import ServerResponse
from src.router import APIRouter

router = APIRouter(prefix="/api/stats")


@router.get("")
async def index(
        ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
        daily_stats: GetUserDailyStatsHandler = Depends(),
        lifetime: LifetimeStatsRepository = Depends(get_lifetime_stats_repo)
):
    return ServerResponse({
        "daily": await daily_stats.handle(ctx),
        "lifetime": await lifetime.get_user_stats(ctx.user_id)
    })
