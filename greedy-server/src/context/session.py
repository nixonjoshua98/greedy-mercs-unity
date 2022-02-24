import secrets

from bson import ObjectId


class Session:
    def __init__(self, user_id: ObjectId):
        self.id = self._generate_id()
        self.user_id: ObjectId = user_id

    @staticmethod
    def _generate_id():
        return f"{secrets.token_urlsafe(128)}".upper()
