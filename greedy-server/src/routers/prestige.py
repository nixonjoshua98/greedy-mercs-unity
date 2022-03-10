from fastapi import Depends

from src.auth import AuthenticatedRequestContext, get_authenticated_context
from src.handlers import (GetStaticDataHandler, GetUserDataHandler,
                          PrestigeHandler, PrestigeResponse)
from src.request_models import PrestigeData
from src.response import ServerResponse
from src.router import APIRouter

router = APIRouter(prefix="/api/prestige")


@router.post("")
async def index(
    data: PrestigeData,
    ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
    handler: PrestigeHandler = Depends(),
    user_data: GetUserDataHandler = Depends(),
    static_data: GetStaticDataHandler = Depends()
):
    resp: PrestigeResponse = await handler.handle(data)

    s_data_resp = await static_data.handle()
    u_data_resp = await user_data.handle(ctx)

    return ServerResponse({
        "prestigePoints": resp.prestige_points,
        "unlockedBounties": resp.unlocked_bounties,
        "userData": u_data_resp.data,
        "staticData": s_data_resp.data
    })
