import secrets

from bson import ObjectId


class Session:
    def __init__(self, uid: ObjectId, device_id: str):
        self.id = self._generate_id(uid, device_id)

        self.user_id: ObjectId = uid
        self.device_id: str = device_id

    @classmethod
    def _generate_id(cls, uid: ObjectId, device_id: str):
        return secrets.token_hex(32)

