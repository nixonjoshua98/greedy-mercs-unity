from __future__ import annotations

from typing import Optional

from bson import ObjectId

from src.redis import RedisClient
from src.request import ServerRequest

from .session import Session

UID_SID_KEY = "Auth:UID-SID/"
SID_UID_KEY = "Auth:SID-UID/"
SID_SJSON_KEY = "Auth:SID-SJSON/"


def authentication_service(request: ServerRequest) -> AuthenticationService:
    return request.app.state.auth_service


class AuthenticationService:
    def __init__(self, redis: RedisClient):
        self._redis: RedisClient = redis

    def set_user_session(self, uid: ObjectId) -> Session:
        sess = Session(uid)
        self._del_user_session_redis(uid)
        self._set_user_session_redis(uid, sess)
        return sess

    def get_user_session(self, sid: str) -> Optional[Session]:
        return self._get_user_session_redis(sid)

    def _del_user_session_redis(self, uid: ObjectId):
        str_sid = self._redis.get(f"{UID_SID_KEY}{uid}")

        self._redis.delete(
            f"{SID_UID_KEY}{str_sid}",
            f"{UID_SID_KEY}{uid}",
            f"{SID_SJSON_KEY}{str_sid}"
        )

    def _get_user_session_redis(self, sid: str) -> Optional[Session]:
        str_uid = self._redis.get(f"{SID_UID_KEY}{sid}")
        str_sid = self._redis.get(f"{UID_SID_KEY}{str_uid}")
        session_json = self._redis.get(f"{SID_SJSON_KEY}{str_sid}")

        return Session.from_json(session_json) if session_json else None

    def _set_user_session_redis(self, uid: ObjectId, sess: Session):
        self._redis.mset({
            f"{UID_SID_KEY}{uid}": sess.id,
            f"{SID_UID_KEY}{sess.id}": str(uid),
            f"{SID_SJSON_KEY}{sess.id}": sess.to_json()
        })
