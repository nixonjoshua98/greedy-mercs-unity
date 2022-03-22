from fastapi import Depends

from src.context import AuthenticatedRequestContext
from src.handlers import GetUserDailyStatsHandler
from src.handlers.auth_handler import get_authenticated_context
from src.handlers.player_stats import GetLifetimeStatsHandler
from src.response import ServerResponse
from src.router import APIRouter

router = APIRouter(prefix="/api/stats")


@router.get("")
async def index(
    ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
    daily_stats: GetUserDailyStatsHandler = Depends(),
    lifetime_stats: GetLifetimeStatsHandler = Depends(),
):
    return ServerResponse({
        "daily": await daily_stats.handle(ctx),
        "lifetime": await lifetime_stats.handle(ctx)
    })
