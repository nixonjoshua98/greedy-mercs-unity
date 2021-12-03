import datetime as dt
import secrets

from bson import ObjectId


class Session:
    def __init__(self, uid: ObjectId, device_id: str):
        self.id = secrets.token_hex(16)

        self.user_id: ObjectId = uid
        self.device_id: str = device_id

        self.created_at = dt.datetime.utcnow()

    def is_valid(self) -> bool:
        return True
