import secrets

from bson import ObjectId

from src import utils


class AuthenticatedSession:
    def __init__(self, user_id: ObjectId, device_id: str, session_id: str):
        self.id: str = session_id
        self.user_id: ObjectId = user_id
        self.device_id: str = device_id

    def __eq__(self, other):
        if isinstance(other, AuthenticatedSession):
            return (self.id == other.id and
                    self.user_id == other.user_id and
                    self.device_id == other.device_id)

        raise NotImplementedError("Attempted to compare invalid objects")

    def __ne__(self, other):
        return not self.__eq__(other)

    def dump(self) -> str:
        return utils.compress({"sid": self.id, "uid": self.user_id, "did": self.device_id})

    @classmethod
    def create(cls, uid: ObjectId, did: str):
        return cls(user_id=uid, device_id=did, session_id=cls.generate_id())

    @classmethod
    def generate_id(cls):
        return f"{secrets.token_urlsafe(128)}".upper()

    @classmethod
    def load(cls, data: str):
        session_dict = utils.decompress(data)

        return cls(
            user_id=ObjectId(session_dict["uid"]),
            device_id=session_dict["did"],
            session_id=session_dict["sid"]
        )
