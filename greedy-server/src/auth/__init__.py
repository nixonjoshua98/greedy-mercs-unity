from __future__ import annotations

from typing import Optional

from bson import ObjectId

from src.redis import RedisClient, execute_redis_pipeline
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
        with execute_redis_pipeline(redis=self._redis) as pipeline:
            self._del_user_session_redis(uid, pipeline=pipeline)
            self._set_user_session_redis(uid, sess, pipeline=pipeline)
        return sess

    def get_user_session(self, sid: str) -> Optional[Session]:
        return self._get_user_session_redis(sid)

    def _del_user_session_redis(self, uid: ObjectId, *, pipeline):
        str_sid = self._redis.get(f"{UID_SID_KEY}{uid}")

        pipeline.delete(f"{SID_UID_KEY}{str_sid}")
        pipeline.delete(f"{UID_SID_KEY}{uid}")
        pipeline.delete(f"{SID_SJSON_KEY}{str_sid}")

    def _get_user_session_redis(self, sid: str) -> Optional[Session]:
        str_uid = self._redis.get(f"{SID_UID_KEY}{sid}")
        str_sid = self._redis.get(f"{UID_SID_KEY}{str_uid}")
        session_json = self._redis.get(f"{SID_SJSON_KEY}{str_sid}")

        return Session.from_json(session_json) if session_json else None

    @classmethod
    def _set_user_session_redis(cls, uid: ObjectId, sess: Session, *, pipeline):
        pipeline.set(f"{UID_SID_KEY}{uid}", sess.id)
        pipeline.set(f"{SID_UID_KEY}{sess.id}", str(uid))
        pipeline.set(f"{SID_SJSON_KEY}{sess.id}", sess.to_json())
