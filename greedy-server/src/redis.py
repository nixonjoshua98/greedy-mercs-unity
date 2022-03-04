from __future__ import annotations

from typing import Optional

from bson import ObjectId

from redis import Redis as _Redis
from src.auth.session import AuthenticatedSession
from src.pymodels.configmodel import RedisConfiguration
from src.request import ServerRequest

SID_UID_KEY = "Auth:SID:UID:"
UID_SID_KEY = "Auth:UID:SID:"
SID_SJSON_KEY = "Auth:SID:SJSON:"


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

        self._client.delete(f"{SID_UID_KEY}{sid}" f"{UID_SID_KEY}{uid}", f"{SID_SJSON_KEY}{sid}")

    def get_user_session(self, sid: str) -> Optional[AuthenticatedSession]:
        """
        Fetch the session associated to the sid.

        :param sid: Session ID

        :return:
            Session object or None
        """
        str_uid = self._client.get(f"{SID_UID_KEY}{sid}")
        str_sid = self._client.get(f"{UID_SID_KEY}{str_uid}")

        if str_sid is not None and (str_sid == sid):
            session_json = self._client.get(f"{SID_SJSON_KEY}{str_sid}")

            return AuthenticatedSession.load(session_json) if session_json else None

    def set_user_session(self, uid: ObjectId, sess: AuthenticatedSession):
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
