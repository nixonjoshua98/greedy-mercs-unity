from __future__ import annotations

from redis import Redis as _Redis
from src.request import ServerRequest
from src.pymodels.configmodel import RedisConfiguration
from bson import ObjectId
from typing import Optional
from src.auth import Session

UID_SID_KEY = "Auth:UID-SID/"
SID_UID_KEY = "Auth:SID-UID/"
SID_SJSON_KEY = "Auth:SID-SJSON/"


class RedisClient:
    def __init__(self, config: RedisConfiguration):
        self._client = _Redis(
            db=config.database,
            host=config.host,
            decode_responses=True
        )

    def update_user_session(self, uid: ObjectId, sess: Session):
        """
        Delete the users old session and create a new one

        :param uid: User ID
        :param sess: Session object
        """
        self.del_user_session(uid)
        self.set_user_session(uid, sess)

    def del_user_session(self, uid: ObjectId):
        """
        Delete all auth session keys required for authentication

        :param uid: User ID
        """
        str_sid = self._client.get(f"{UID_SID_KEY}{uid}")

        self._client.delete(
            f"{SID_UID_KEY}{str_sid}",
            f"{UID_SID_KEY}{uid}",
            f"{SID_SJSON_KEY}{str_sid}"
        )

    def get_user_session(self, sid: str) -> Optional[Session]:
        """
        Fetch the session associated to the sid.

        Note: We must go through the user lookup instead of directly to the session lookup to avoid old sessions
                being counted as valid when they shouldn't

        :param sid: Session ID

        :return:
            Session object or None
        """
        str_uid = self._client.get(f"{SID_UID_KEY}{sid}")
        str_sid = self._client.get(f"{UID_SID_KEY}{str_uid}")
        session_json = self._client.get(f"{SID_SJSON_KEY}{str_sid}")

        return Session.load(session_json) if session_json else None

    def set_user_session(self, uid: ObjectId, sess: Session):
        """
        Set the required keys for authentication session lookup

        :param uid: User ID
        :param sess: Session Object
        """
        self._client.mset({
            f"{UID_SID_KEY}{uid}": sess.id,
            f"{SID_UID_KEY}{sess.id}": str(uid),
            f"{SID_SJSON_KEY}{sess.id}": sess.dump()
        })


def redis_client(request: ServerRequest) -> RedisClient:
    return request.app.state.redis_client
