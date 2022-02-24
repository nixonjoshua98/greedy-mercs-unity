from fastapi import Depends

from src.cache import MemoryCache, memory_cache
from src.context import RequestContext, Session
from src.mongo.repositories.accounts import (AccountsRepository,
                                             accounts_repository)
from src.pymodels import BaseModel
from src.routing import APIRouter, ServerResponse
from src.routing.handlers.accounts import CreateAccountHandler

from ..handlers.data.userdata import GetUserDataHandler, UserDataResponse

router = APIRouter()


class LoginModel(BaseModel):
    device_id: str


@router.post("/")
async def player_login(
    data: LoginModel,
    ctx: RequestContext = Depends(),
    user_data_handler: GetUserDataHandler = Depends(),
    create_account: CreateAccountHandler = Depends(),
    mem_cache: MemoryCache = Depends(memory_cache),
    acc_repo: AccountsRepository = Depends(accounts_repository),
):
    user = await acc_repo.get_user_by_device_id(data.device_id)

    if user is None:
        user = await create_account.handle(data.device_id)

    data_resp: UserDataResponse = await user_data_handler.handle(user.id, ctx.prev_daily_reset)

    mem_cache.set_session(session := Session(user.id))

    return ServerResponse({
        "userId": user.id,
        "sessionId": session.id,
        "userData": data_resp.data
    })
