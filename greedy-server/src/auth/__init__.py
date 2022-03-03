from __future__ import annotations

from typing import Optional

from bson import ObjectId

from src.redis import RedisClient
from src.request import ServerRequest

from .session import Session


def authentication_service(request: ServerRequest) -> AuthenticationService:
    return request.app.state.auth_service


class AuthenticationService:
    def __init__(self, redis: RedisClient):
        self._redis: RedisClient = redis

    def set_user_session(self, uid: ObjectId) -> Session:
        self._redis.update_user_session(uid, sess := Session(uid))
        return sess

    def get_user_session(self, sid: str) -> Optional[Session]:
        return self._redis.get_user_session(sid)
