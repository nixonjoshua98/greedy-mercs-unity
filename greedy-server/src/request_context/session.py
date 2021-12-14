import secrets

from bson import ObjectId


class Session:
    def __init__(self, user_id: ObjectId, device_id: str):
        self.id = self._generate_id(user_id, device_id)

        self.user_id: ObjectId = user_id
        self.device_id: str = device_id

    @staticmethod
    def _generate_id(user_id: ObjectId, device_id: str):
        return f"{user_id}{secrets.token_urlsafe(8)}{device_id}".upper()
