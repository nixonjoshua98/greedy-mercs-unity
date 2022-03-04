from __future__ import annotations

from typing import Optional

from bson import ObjectId

from redis import Redis as _Redis
from src.auth.session import Session
from src.pymodels.configmodel import RedisConfiguration
from src.request import ServerRequest

UID_SID_KEY = "Auth:UID:SID/"
SID_SJSON_KEY = "Auth:SID:SJSON/"


class RedisClient:
    def __init__(self, config: RedisConfiguration):
        self._client = _Redis(
            db=config.database,
            host=config.host,
            decode_responses=True
        )

    def del_user_session(self, uid: ObjectId):
        """
        Delete all auth session keys required for authentication

        :param uid: User ID
        """
        sid = self._client.get(f"{UID_SID_KEY}{uid}")

        self._client.delete(
            f"{UID_SID_KEY}{uid}",
            f"{SID_SJSON_KEY}{sid}"
        )

    def get_user_session(self, sid: str) -> Optional[Session]:
        """
        Fetch the session associated to the sid.

        :param sid: Session ID

        :return:
            Session object or None
        """
        session_json = self._client.get(f"{SID_SJSON_KEY}{sid}")

        return Session.load(session_json) if session_json else None

    def set_user_session(self, uid: ObjectId, sess: Session):
        """
        Set the required keys for authentication session lookup

        :param uid: User ID
        :param sess: Session Object
        """
        self._client.mset({
            f"{UID_SID_KEY}{uid}": sess.id,
            f"{SID_SJSON_KEY}{sess.id}": sess.dump()
        })


def redis_client(request: ServerRequest) -> RedisClient:
    return request.app.state.redis_client
