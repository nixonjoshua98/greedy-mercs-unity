from fastapi import Depends

from src.handlers import LoginHandler, LoginResponse
from src.request_models import LoginModel
from src.response import ServerResponse
from src.router import APIRouter

router = APIRouter(prefix="/api/login")


@router.post("")
async def index(
    data: LoginModel,
    handler: LoginHandler = Depends()
):
    resp: LoginResponse = await handler.handle(data)

    return ServerResponse({
        "userId": resp.user_id,
        "sessionId": resp.session.id,
        "userData": resp.user_data
    })
