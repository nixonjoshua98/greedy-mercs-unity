from typing import Optional

from fastapi import Depends

from src.common.constants import StatusCodes
from src.dependencies import (get_auth_sessions_repo, get_auth_token_header,
                              get_device_id_header)
from src.exceptions import ServerException
from src.mongo.sessions import SessionModel, SessionRepository

from .context import AuthenticatedRequestContext


async def get_authenticated_context(
    device_id: str = Depends(get_device_id_header),
    auth_key: str = Depends(get_auth_token_header),
    sessions=Depends(get_auth_sessions_repo)
):
    handler = AuthenticationHandler(sessions)

    return await handler.handle(auth_key, device_id)


class AuthenticationHandler:
    def __init__(self, sessions: SessionRepository):
        self._sessions = sessions

    async def handle(self, session_id: str, device_id: str) -> AuthenticatedRequestContext:

        if session_id is None or device_id is None:
            raise ServerException(401, "Unauthorized")

        session: Optional[SessionModel] = await self._sessions.get_session(session_id)

        if session is None:  # Force a restart but do not clear data
            raise ServerException(401, "Unauthorized")

        elif (not session.is_valid) or (device_id != session.device_id):
            await self._sessions.invalidate_all_user_sessions(session.user_id)
            raise ServerException(StatusCodes.INVALIDATE_CLIENT, "Unauthorized")

        return AuthenticatedRequestContext(uid=session.user_id)
