from __future__ import annotations

from typing import Optional

from bson import ObjectId

from src.redis import RedisClient

from .session import Session


class AuthenticationService:
    def __init__(self, redis: RedisClient):
        self._redis: RedisClient = redis

    def set_user_session(self, uid: ObjectId) -> Session:
        sess = Session(uid)
        self._redis.del_user_session(uid)
        self._redis.set_user_session(uid, sess)
        return sess

    def get_user_session(self, sid: str) -> Optional[Session]:
        return self._redis.get_user_session(sid)
