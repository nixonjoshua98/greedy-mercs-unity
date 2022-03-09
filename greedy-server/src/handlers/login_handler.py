from typing import Optional

from bson import ObjectId
from fastapi import Depends, Header, HTTPException

from src.auth import (AuthenticatedSession, AuthenticationService,
                      RequestContext, authentication_service)
from src.common.constants import Headers as HeaderConstants
from src.handlers import (AccountCreationRequest, AccountCreationResponse,
                          CreateAccountHandler, GetUserDataHandler,
                          UserDataResponse)
from src.handlers.abc import BaseHandler
from src.mongo.repositories.accounts import (AccountModel, AccountsRepository,
                                             accounts_repository)
from src.mongo.repositories.units import (CharacterUnitsRepository,
                                          units_repository)
from src.pymodels import BaseModel
from src.request_models import LoginData


class LoginResponse(BaseModel):
    user_id: ObjectId
    user_data: dict
    session_id: str


class LoginHandler(BaseHandler):
    def __init__(
        self,
        ctx: RequestContext = Depends(),
        device_id: str = Header(None, alias=HeaderConstants.DEVICE_ID),
        user_data_handler: GetUserDataHandler = Depends(),
        create_account_handler: CreateAccountHandler = Depends(),
        units_repo: CharacterUnitsRepository = Depends(units_repository),
        auth: AuthenticationService = Depends(authentication_service),
        accounts: AccountsRepository = Depends(accounts_repository)
    ):
        self.device_id: Optional[str] = device_id

        self._ctx = ctx
        self._accounts = accounts
        self._create_account_handler = create_account_handler
        self._user_data_handler = user_data_handler
        self._auth = auth
        self.units_repo = units_repo

        self.account: Optional[AccountModel] = None

    async def handle(self, model: LoginData) -> LoginResponse:
        if self.device_id is None:
            raise HTTPException(400, detail="Error")

        await self._get_or_create_account()

        data_resp: UserDataResponse = await self._user_data_handler.handle(self.account.id, self._ctx)

        session = self._create_auth_session()

        return LoginResponse(user_id=self.account.id, session_id=session.id, user_data=data_resp.data)

    def _create_auth_session(self) -> AuthenticatedSession:
        session = AuthenticatedSession.create(self.account.id, self.device_id)

        self._auth.set_session(session)

        return session

    async def _get_or_create_account(self):

        self.account = await self._accounts.get_user_by_device_id(self.device_id)

        if self.account is None:
            request = AccountCreationRequest(device_id=self.device_id)

            response: AccountCreationResponse = await self._create_account_handler.handle(request)

            self.account = response.account

            if self.account is None:
                raise HTTPException(500, detail="Account creation has failed")
