from typing import Optional

from fastapi import Depends, Header

from src.common.constants import Headers as HeaderStrings
from src.common.constants import StatusCodes
from src.exceptions import ServerException

from .context import AuthenticatedRequestContext
from .service import AuthenticationService, authentication_service
from .session import AuthenticatedSession


async def get_authenticated_context(
    device_id: str = Header(default=None, alias=HeaderStrings.DEVICE_ID),
    auth_key: str = Header(default=None, alias=HeaderStrings.AUTH_KEY),
    auth_service: AuthenticationService = Depends(authentication_service)
):
    handler = AuthenticationHandler(auth_service)

    return await handler.handle(auth_key, device_id)


class AuthenticationHandler:
    def __init__(self, auth_service: AuthenticationService):
        self.auth_service: AuthenticationService = auth_service

    async def handle(self, auth_key: str, device_id: str) -> AuthenticatedRequestContext:

        # Potentially malicious request
        if auth_key is None or device_id is None:
            raise ServerException(401, "Unauthorized")

        session: Optional[AuthenticatedSession] = self.auth_service.get_session(auth_key)

        # Session is invalid. Could be malicious or a server issue
        if session is None:
            raise ServerException(401, "Unauthorized")

        # Same session but different device. We should invalidate the client
        elif device_id != session.device_id:
            raise ServerException(StatusCodes.INVALIDATE_CLIENT, "Unauthorized")

        # Fetch the current session associated to the user from the provided session
        current_session: Optional[AuthenticatedSession] = self.auth_service.get_current_session(session.user_id)

        # Attempted to login with an old session. We should invalidate the client data
        # We should not invalidate the 'current_session' as the session may not
        # actually belong to the user which made this request
        if current_session is None or (current_session != session):
            raise ServerException(StatusCodes.INVALIDATE_CLIENT, "Unauthorized")

        return AuthenticatedRequestContext(uid=session.user_id)
