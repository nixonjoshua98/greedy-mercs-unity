from fastapi import Depends

from src.handlers import LoginHandler, LoginResponse
from src.request_models import LoginData
from src.response import ServerResponse
from src.router import APIRouter

router = APIRouter(prefix="/api/login")


@router.post("")
async def index(
    model: LoginData,
    handler: LoginHandler = Depends(),
):
    resp: LoginResponse = await handler.handle(model)

    return ServerResponse({
        #"userId": resp.user_id,
        #"sessionId": resp.session_id,
        "userData": resp.user_data["bountyData"]
    })
