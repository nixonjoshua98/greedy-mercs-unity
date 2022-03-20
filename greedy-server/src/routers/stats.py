from fastapi import Depends

from src.auth.handler import (AuthenticatedRequestContext,
                              get_authenticated_context)
from src.handlers import GetUserDailyStatsHandler
from src.response import ServerResponse
from src.router import APIRouter

router = APIRouter(prefix="/api/stats")


@router.get("/daily")
async def get_daily_stats(
        ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
        handler: GetUserDailyStatsHandler = Depends()
):
    resp = await handler.handle(ctx)

    return ServerResponse(resp)
