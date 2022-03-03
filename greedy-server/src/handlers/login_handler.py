import dataclasses
from typing import Optional

from bson import ObjectId
from fastapi import Depends, HTTPException

from src.auth import Session, AuthenticationService, authentication_service
from src.context import RequestContext
from src.handlers import (CreateAccountHandler, GetUserDataHandler,
                          UserDataResponse, AccountCreationResponse)
from src.handlers.abc import BaseHandler, BaseResponse
from src.mongo.repositories.accounts import (AccountModel, AccountsRepository,
                                             accounts_repository)
from src.request_models import LoginModel


@dataclasses.dataclass()
class LoginResponse(BaseResponse):
    user_id: ObjectId
    user_data: dict
    session: Session


class LoginHandler(BaseHandler):
    def __init__(
        self,
        ctx: RequestContext = Depends(),
        user_data_handler: GetUserDataHandler = Depends(),
        create_account_handler: CreateAccountHandler = Depends(),
        auth: AuthenticationService = Depends(authentication_service),
        accounts: AccountsRepository = Depends(accounts_repository)
    ):
        self._ctx = ctx
        self._accounts = accounts
        self._create_account_handler = create_account_handler
        self._user_data_handler = user_data_handler
        self._auth = auth

        self.account: Optional[AccountModel] = None

    async def handle(self, data: LoginModel) -> LoginResponse:
        await self._get_or_create_account(data)

        data_resp: UserDataResponse = await self._user_data_handler.handle(self.account.id, self._ctx)

        session = self._create_auth_session()

        return LoginResponse(
            user_id=self.account.id,
            session=session,
            user_data=data_resp.data
        )

    def _create_auth_session(self) -> Session:
        session = self._auth.set_user_session(self.account.id)

        return session

    async def _get_or_create_account(self, data: LoginModel):

        self.account = await self._accounts.get_user_by_device_id(data.device_id)

        if self.account is None:
            response: AccountCreationResponse = await self._create_account_handler.handle(data)

            self.account = response.account

            if self.account is None:
                raise HTTPException(500, detail="Account creation has failed")



