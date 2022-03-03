from __future__ import annotations

from typing import Optional
from bson import ObjectId

from .session import Session
from src.redis import RedisClient, RedisPrefix
from src.request import ServerRequest


def authentication_service(request: ServerRequest) -> AuthenticationService:
    return request.app.state.auth_service


class AuthenticationService:
    def __init__(self, redis: RedisClient):
        self.__redis: RedisClient = redis

        self.__sessionid2user: dict[str, ObjectId] = dict()
        self.__userid2session: dict[ObjectId, Session] = dict()

    def set_user_session(self, uid: ObjectId) -> Session:
        self.del_user_session(uid)

        sess = Session(uid)

        self.__redis.set(f"{RedisPrefix.USER_2_SESSION}/{uid}", sess.to_json())
        self.__redis.set(f"{RedisPrefix.SESSION_2_USER}/{sess.id}", str(uid))

        return sess

    def get_user_session(self, sid: str) -> Optional[Session]:
        str_uid = self.__redis.get(f"{RedisPrefix.SESSION_2_USER}/{sid}")

        if str_uid:
            session_json = self.__redis.get(f"{RedisPrefix.USER_2_SESSION}/{str_uid}")

            if session_json:
                return Session.from_json(session_json)

    def del_user_session(self, uid: ObjectId):
        session_json = self.__redis.get(f"{RedisPrefix.USER_2_SESSION}/{uid}")

        if session_json:
            sess = Session.from_json(session_json)

            self.__redis.delete(f"{RedisPrefix.USER_2_SESSION}/{uid}")
            self.__redis.delete(f"{RedisPrefix.SESSION_2_USER}/{sess.id}")
