from fastapi import Depends

from src.cache import MemoryCache, memory_cache
from src.mongo.repositories.accounts import (AccountsRepository,
                                             accounts_repository)
from src.pymodels import BaseModel
from src.request_context.session import Session
from src.routing import APIRouter, ServerResponse, ServerRequest
from src.routing.handlers.accounts import CreateAccountHandler
from src.mongo.repositories.units import units_repository
router = APIRouter()


class LoginModel(BaseModel):
    device_id: str


@router.post("/")
async def player_login(
    request: ServerRequest,
    data: LoginModel,
    create_account: CreateAccountHandler = Depends(),
    mem_cache: MemoryCache = Depends(memory_cache),
    acc_repo: AccountsRepository = Depends(accounts_repository),
):
    user = await acc_repo.get_user_by_did(data.device_id)

    if user is None:
        user = await create_account.handle(data.device_id)

    await units_repository(request).insert_units(user.id, [0, 2])

    mem_cache.set_session(session := Session(user.id, data.device_id))

    return ServerResponse({"userId": user.id, "sessionId": session.id})
