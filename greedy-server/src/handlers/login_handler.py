import secrets
from typing import Optional

from bson import ObjectId
from fastapi import Depends, HTTPException

from src.dependencies import get_auth_sessions_repo, get_device_id_header
from src.repositories.accounts import (AccountModel, AccountsRepository,
                                       get_accounts_repository)
from src.repositories.sessions import SessionModel, SessionRepository
from src.request_models import LoginRequestModel
from src.shared_models import BaseModel


class LoginResponse(BaseModel):
    user_id: ObjectId
    session_id: str


class LoginHandler:
    def __init__(
        self,
        device_id: str = Depends(get_device_id_header),
        accounts=Depends(get_accounts_repository),
        sessions=Depends(get_auth_sessions_repo)
    ):
        self.device_id: str = device_id
        self._sessions: SessionRepository = sessions
        self._accounts: AccountsRepository = accounts

    async def handle(self, model: LoginRequestModel) -> LoginResponse:
        account: Optional[AccountModel] = await self._accounts.get_user_by_device_id(model.device_id)

        if account is None:
            raise HTTPException(500, "Login failed - Account not found")

        session = SessionModel(
            session_id=secrets.token_urlsafe(128).upper(),
            device_id=self.device_id,
            user_id=account.id,
            is_valid=True
        )

        await self._sessions.invalidate_all_user_sessions(account.id)
        await self._sessions.insert_session(session)

        return LoginResponse(user_id=account.id, session_id=session.session_id)
