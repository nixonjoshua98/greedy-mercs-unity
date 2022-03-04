import json
import secrets

from bson import ObjectId


class Session:
    def __init__(self, uid: ObjectId, *, sid: str = None):
        self.id: str = sid if sid else self.generate_id(uid)
        self.user_id: ObjectId = uid

    @staticmethod
    def generate_id(uid: ObjectId):
        return f"{secrets.token_urlsafe(128)}{uid}".upper()

    def dump(self) -> str:
        return json.dumps({"sid": self.id, "uid": self.user_id}, default=str)

    @classmethod
    def load(cls, data: str):
        session_dict = json.loads(data)

        return cls(uid=ObjectId(session_dict["uid"]), sid=session_dict["sid"])
