from .session import Session
from .service import AuthenticationService

from src.request import ServerRequest as _ServerRequest


def authentication_service(request: _ServerRequest) -> AuthenticationService:
    return request.app.state.auth_service


__all__ = (
    "Session",
    "AuthenticationService",
    "authentication_service"
)
