from fastapi import Depends

from src.handlers import (CreateAccountHandler, GetUserDataHandler,
                          LoginHandler, LoginResponse)
from src.repositories.accounts import (AccountModel, AccountsRepository,
                                       get_accounts_repository)
from src.request_models import LoginRequestModel
from src.response import ServerResponse
from src.router import APIRouter

router = APIRouter(prefix="/api/login")


@router.post("")
async def index(
    model: LoginRequestModel,
    # Handlers #
    _login: LoginHandler = Depends(),
    _user_data: GetUserDataHandler = Depends(),
    _create_account: CreateAccountHandler = Depends(),
    # Repositories #
    _accounts: AccountsRepository = Depends(get_accounts_repository)
):
    user: AccountModel = await _accounts.get_user_by_device_id(model.device_id)

    if user is None:
        await _create_account.handle(model)

    resp: LoginResponse = await _login.handle(model)

    user_data = await _user_data.handle(resp.user_id)

    return ServerResponse({"token": resp.session_id, "userData": user_data.data})
