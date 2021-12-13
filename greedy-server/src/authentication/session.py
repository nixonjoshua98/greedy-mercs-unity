import secrets

from bson import ObjectId


class Session:
    def __init__(self, user_id: ObjectId, device_id: str):
        self.id = secrets.token_urlsafe(32)

        self.user_id: ObjectId = user_id
        self.device_id: str = device_id
