from fastapi import Depends

from src.handlers import PrestigeHandler, PrestigeResponse
from src.request_models import PrestigeData
from src.response import ServerResponse
from src.router import APIRouter

router = APIRouter(prefix="/api/prestige")


@router.post("/")
async def index(
    data: PrestigeData,
    handler: PrestigeHandler = Depends()
):
    resp: PrestigeResponse = await handler.handle(data)

    return ServerResponse({
        "prestigePoints": resp.prestige_points,
        "unlockedBounties": resp.unlocked_bounties
    })
