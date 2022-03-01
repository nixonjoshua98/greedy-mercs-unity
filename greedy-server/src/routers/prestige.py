from fastapi import Depends

from src.handlers import PrestigeHandler, PrestigeResponse
from src.request_models import PrestigeData
from src.response import ServerResponse
from src.router import APIRouter
from src.handlers import GetUserDataHandler, GetStaticDataHandler
from src.context import AuthenticatedRequestContext, inject_authenticated_context

router = APIRouter(prefix="/api/prestige")


@router.post("/")
async def index(
    data: PrestigeData,
    ctx: AuthenticatedRequestContext = Depends(inject_authenticated_context),
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
