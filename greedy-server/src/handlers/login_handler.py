from typing import Optional

from bson import ObjectId
from fastapi import Depends, HTTPException

from src.auth import AuthenticatedSession, authentication_service
from src.dependencies import get_device_id
from src.handlers import (AccountCreationRequest, AccountCreationResponse,
                          CreateAccountHandler, GetUserDataHandler,
                          GetUserDataResponse)
from src.handlers.abc import BaseHandler
from src.mongo.repositories.accounts import AccountModel, accounts_repository
from src.mongo.repositories.units import units_repository
from src.pymodels import BaseModel
from src.request_models import LoginData


class LoginResponse(BaseModel):
    user_id: ObjectId
    user_data: dict
    session_id: str


class LoginHandler(BaseHandler):
    def __init__(
        self,
        # Client Data #
        device_id: str = Depends(get_device_id),
        # Handlers #
        user_data_handler: GetUserDataHandler = Depends(),
        create_account_handler: CreateAccountHandler = Depends(),
        # Repositories #
        units_repo=Depends(units_repository),
        auth=Depends(authentication_service),
        accounts=Depends(accounts_repository)
    ):
        self.device_id: str = device_id

        self._accounts = accounts
        self._create_account_handler = create_account_handler
        self._user_data_handler = user_data_handler
        self._auth = auth
        self.units_repo = units_repo

        self.account: Optional[AccountModel] = None

    async def handle(self, _model: LoginData) -> LoginResponse:

        # Should be removed out and account creation should be elsewhere
        # If an account does not exist then we should either return a response or raise an error
        await self._get_or_create_account()

        # These two should be moved out
        data_resp: GetUserDataResponse = await self._user_data_handler.handle(self.account.id)
        await self.units_repo.insert_units(self.account.id, [0, 1, 2, 3])

        session = self._create_auth_session()

        return LoginResponse(user_id=self.account.id, session_id=session.id, user_data=data_resp.data)

    async def _get_or_create_account(self):
        self.account = await self._accounts.get_user_by_device_id(self.device_id)

        if self.account is None:
            request = AccountCreationRequest(device_id=self.device_id)

            response: AccountCreationResponse = await self._create_account_handler.handle(request)

            self.account = response.account

            if self.account is None:
                raise HTTPException(500, "Account creation has failed")

    def _create_auth_session(self) -> AuthenticatedSession:
        """
        Create an authenticated session in cache then return it

        :return:
            Authenticated session which was created
        """
        session = AuthenticatedSession.create(self.account.id, self.device_id)

        self._auth.set_session(session)

        return session
