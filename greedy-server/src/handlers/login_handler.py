from typing import Optional

from bson import ObjectId
from fastapi import Depends, HTTPException

from src.auth import AuthenticatedSession, authentication_service
from src.dependencies import get_device_id
from src.handlers.abc import BaseHandler
from src.mongo.accounts import AccountModel, accounts_repository
from src.pymodels import BaseModel
from src.request_models import LoginData


class LoginResponse(BaseModel):
    user_id: ObjectId
    session_id: str


class LoginHandler(BaseHandler):
    def __init__(
        self,
        # Client Data #
        device_id: str = Depends(get_device_id),
        # Repositories #
        auth=Depends(authentication_service),
        accounts=Depends(accounts_repository)
    ):
        self.device_id: str = device_id

        self._accounts = accounts
        self._auth = auth

    async def handle(self, _model: LoginData) -> LoginResponse:
        account: Optional[AccountModel] = await self._accounts.get_user_by_device_id(_model.device_id)

        if account is None:
            raise HTTPException(500, "Login failed - Account not found")

        session = self._create_auth_session(account)

        return LoginResponse(user_id=account.id, session_id=session.id)

    def _create_auth_session(self, account: AccountModel) -> AuthenticatedSession:
        """
        Create an authenticated session in cache then return it

        :return:
            Authenticated session which was created
        """
        session = AuthenticatedSession.create(account.id, self.device_id)

        self._auth.set_session(session)

        return session
