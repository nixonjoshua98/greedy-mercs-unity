from typing import Optional

from fastapi import Depends, HTTPException

from src.request import ServerRequest
from src.request import ServerRequest as _ServerRequest

from .context import AuthenticatedRequestContext, RequestContext
from .service import AuthenticationService
from .session import AuthenticatedSession


def authentication_service(request: _ServerRequest) -> AuthenticationService:
    return request.app.state.auth_service


def get_context(request: ServerRequest) -> RequestContext:
    return RequestContext(request=request)


def get_authenticated_context(
        request: ServerRequest,
        auth: AuthenticationService = Depends(authentication_service),
) -> AuthenticatedRequestContext:

    key: Optional[str] = request.headers.get("authentication")

    # Header was not provided or session was not found
    if key is None or (sess := auth.get_user_session(key)) is None:
        raise HTTPException(401, detail="Unauthorized")

    return AuthenticatedRequestContext(uid=sess.user_id, request=request)


__all__ = (
    "AuthenticatedSession",
    "AuthenticationService",
    "authentication_service",
    "AuthenticatedRequestContext",
    "RequestContext",
    "get_authenticated_context",
    "get_context"
)
