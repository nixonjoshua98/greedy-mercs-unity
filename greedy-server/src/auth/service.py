from __future__ import annotations

from typing import Optional

from bson import ObjectId

from src.redis import RedisClient

from .session import AuthenticatedSession


class AuthenticationService:
    def __init__(self, redis: RedisClient):
        self._redis: RedisClient = redis

    def set_user_session(self, uid: ObjectId) -> AuthenticatedSession:
        sess = AuthenticatedSession(uid)
        self._redis.set_user_session(uid, sess)
        return sess

    def get_user_session(self, sid: str) -> Optional[AuthenticatedSession]:
        return self._redis.get_user_session(sid)
