from typing import Optional

from fastapi import Depends

from src.application import Application
from src.common.types import StatusCodes
from src.context import AuthenticatedRequestContext
from src.dependencies import (get_application, get_auth_sessions_repo,
                              get_auth_token_header, get_device_id_header)
from src.exceptions import ServerException
from src.repositories.sessions import SessionModel, SessionRepository


async def get_authenticated_context(
    app: Application = Depends(get_application),
    device_id=Depends(get_device_id_header),
    auth_key=Depends(get_auth_token_header),
    sessions=Depends(get_auth_sessions_repo)
):
    handler = AuthenticationHandler(sessions=sessions, app=app)

    return await handler.handle(auth_key, device_id)


class AuthenticationHandler:
    def __init__(self, sessions: SessionRepository, app: Application):
        self._sessions = sessions
        self._app = app

    async def handle(self, session_id: str, device_id: str) -> AuthenticatedRequestContext:

        if session_id is None or device_id is None:
            raise ServerException(401, "Unauthorized")

        session: Optional[SessionModel] = await self._sessions.get_session(session_id)

        if session is None:  # Force a restart but do not clear data
            raise ServerException(401, "Unauthorized")

        elif (not session.is_valid) or (device_id != session.device_id):
            raise ServerException(StatusCodes.INVALIDATE_CLIENT, "Unauthorized")

        return AuthenticatedRequestContext(app=self._app, uid=session.user_id)
