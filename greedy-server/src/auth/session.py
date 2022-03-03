import secrets

from bson import ObjectId
from typing import TypedDict
import json


class SessionDict(TypedDict):
    session_id: str
    user_id: str


class Session:
    def __init__(self, uid: ObjectId, *, sid: str = None):
        self.id: str = secrets.token_urlsafe(128).upper() if sid is None else sid
        self.user_id: ObjectId = uid

    def to_json(self) -> str:
        return json.dumps({
            "session_id": self.id,
            "user_id": self.user_id
        }, default=str)

    @classmethod
    def from_json(cls, data: str):
        session_dict: SessionDict = json.loads(data)

        return cls(uid=ObjectId(session_dict["user_id"]), sid=session_dict["session_id"])
