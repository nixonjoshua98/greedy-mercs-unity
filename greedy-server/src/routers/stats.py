from fastapi import Depends

from src.context import AuthenticatedRequestContext
from src.handlers import GetUserDailyStatsHandler
from src.handlers.auth_handler import get_authenticated_context
from src.handlers.player_stats import (GetLifetimeStatsHandler,
                                       UpdateLifetimeStatsHandler)
from src.response import ServerResponse
from src.router import APIRouter
from src.shared_models import BaseModel, PlayerStatsModel

router = APIRouter(prefix="/api/stats")


class UpdateLifetimeStatsBody(BaseModel):
    stat_changes: PlayerStatsModel


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


@router.post("/lifetime")
async def update_lifetime_stats(
        model: UpdateLifetimeStatsBody,
        ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
        handler: UpdateLifetimeStatsHandler = Depends(),
):
    resp = await handler.handle(ctx.user_id, model.stat_changes)

    return ServerResponse(resp)
