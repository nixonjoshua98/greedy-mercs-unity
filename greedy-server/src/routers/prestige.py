from fastapi import Depends

from src.context import AuthenticatedRequestContext, RequestContext
from src.handlers import (GetStaticDataHandler, GetUserDataHandler,
                          PrestigeHandler, PrestigeResponse)
from src.handlers.auth_handler import get_authenticated_context
from src.request_models import PrestigeRequestModel
from src.response import ServerResponse
from src.router import APIRouter

router = APIRouter(prefix="/api/prestige")


@router.post("")
async def index(
    data: PrestigeRequestModel,
    ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
    handler: PrestigeHandler = Depends(),
    user_data: GetUserDataHandler = Depends(),
    static_data: GetStaticDataHandler = Depends()
):
    resp: PrestigeResponse = await handler.handle(data)

    s_data_resp = await static_data.handle()
    u_data_resp = await user_data.handle(ctx.user_id)

    return ServerResponse({
        "prestigePoints": resp.prestige_points,
        "unlockedBounties": resp.unlocked_bounties,
        "userData": u_data_resp.data,
        "staticData": s_data_resp
    })
