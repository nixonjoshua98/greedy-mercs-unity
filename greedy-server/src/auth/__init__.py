from typing import Optional

from fastapi import Depends, HTTPException

from src.request import ServerRequest
from src.request import ServerRequest as _ServerRequest

from .context import AuthenticatedRequestContext, RequestContext
from .service import AuthenticationService
from .session import Session


def authentication_service(request: _ServerRequest) -> AuthenticationService:
    return request.app.state.auth_service


async def inject_authenticated_context(
    request: ServerRequest,
    auth: AuthenticationService = Depends(authentication_service),
) -> AuthenticatedRequestContext:
    key: Optional[str] = request.headers.get("authentication")

    # Header was not provided or session was not found
    if key is None or (sess := auth.get_user_session(key)) is None:
        raise HTTPException(401, detail="Unauthorized")

    return AuthenticatedRequestContext(uid=sess.user_id)


__all__ = (
    "Session",
    "AuthenticationService",
    "authentication_service",
    "AuthenticatedRequestContext",
    "RequestContext",
    "inject_authenticated_context"
)
