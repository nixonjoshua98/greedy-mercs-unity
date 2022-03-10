from fastapi import Depends

from src.handlers import (CreateAccountHandler, GetUserDataHandler,
                          LoginHandler, LoginResponse)
from src.mongo.accounts import (AccountModel, AccountsRepository,
                                accounts_repository)
from src.mongo.units import CharacterUnitsRepository, units_repository
from src.request_models import LoginData
from src.response import ServerResponse
from src.router import APIRouter

router = APIRouter(prefix="/api/login")


@router.post("")
async def index(
    model: LoginData,
    # Handlers #
    _login: LoginHandler = Depends(),
    _user_data: GetUserDataHandler = Depends(),
    _create_account: CreateAccountHandler = Depends(),
    # Repositories =
    _accounts: AccountsRepository = Depends(accounts_repository),
    _units_repo: CharacterUnitsRepository = Depends(units_repository)

):
    user: AccountModel = await _accounts.get_user_by_device_id(model.device_id)

    if user is None:
        await _create_account.handle(model)

    resp: LoginResponse = await _login.handle(model)

    await _units_repo.insert_units(resp.user_id, [0, 1, 2, 3])

    user_data = await _user_data.handle(resp.user_id)

    return ServerResponse({"sessionId": resp.session_id, "userData": user_data.data})
