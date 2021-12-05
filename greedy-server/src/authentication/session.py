import datetime as dt
import random

from bson import ObjectId


class Session:
    def __init__(self, uid: ObjectId, device_id: str):
        self.id = self.generate_id(uid, device_id)

        self.user_id: ObjectId = uid
        self.device_id: str = device_id

        self.created_at = dt.datetime.utcnow()

    def is_valid(self) -> bool:
        return True

    @classmethod
    def generate_id(cls, uid: ObjectId, device_id: str):
        return "".join(random.sample(f"{uid}{device_id}", 32))

