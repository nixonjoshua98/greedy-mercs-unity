import secrets
from typing import Optional

from bson import ObjectId
from fastapi import Depends, HTTPException

from src.dependencies import get_device_id_header
from src.mongo.accounts import (AccountModel, AccountsRepository, SessionModel,
                                accounts_repository)
from src.pymodels import BaseModel
from src.request_models import LoginData


class LoginResponse(BaseModel):
    user_id: ObjectId
    session_id: str


class LoginHandler:
    def __init__(
        self,
        device_id: str = Depends(get_device_id_header),
        accounts=Depends(accounts_repository)
    ):
        self.device_id: str = device_id

        self._accounts: AccountsRepository = accounts

    async def handle(self, model: LoginData) -> LoginResponse:
        account: Optional[AccountModel] = await self._accounts.get_user_by_device_id(model.device_id)

        if account is None:
            raise HTTPException(500, "Login failed - Account not found")

        session = SessionModel(id=secrets.token_urlsafe(128).upper(), device_id=self.device_id)

        await self._accounts.update_user_session(account.id, session)

        return LoginResponse(user_id=account.id, session_id=session.id)
