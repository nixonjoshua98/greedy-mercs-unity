from __future__ import annotations

from typing import TYPE_CHECKING, Optional

from bson import ObjectId

from src.common.constants import RedisKeys
from src.request import ServerRequest as _ServerRequest

from .session import AuthenticatedSession

if TYPE_CHECKING:
    from redis import Redis


def authentication_service(request: _ServerRequest) -> AuthenticationService:
    return request.app.state.auth_service


class AuthenticationService:
    def __init__(self, redis: Redis):
        self._redis: Redis = redis

    def get_session(self, sid: str) -> Optional[AuthenticatedSession]:
        val: Optional[str] = self._redis.get(f"{RedisKeys.AUTH_SESSION}{sid}")

        return None if val is None else AuthenticatedSession.load(val)

    def get_current_session(self, uid: ObjectId) -> AuthenticatedSession:
        sid: Optional[str] = self._redis.get(f"{RedisKeys.USER_SESSION}{uid}")

        return self.get_session(sid) if sid else None

    def set_session(self, session: AuthenticatedSession):
        self._redis.mset({
            f"{RedisKeys.USER_SESSION}{session.user_id}": session.id,
            f"{RedisKeys.AUTH_SESSION}{session.id}": session.dump()}
        )
