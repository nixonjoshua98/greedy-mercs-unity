
from motor.motor_asyncio import AsyncIOMotorClient

from .children import _Users, _Bounties, _Items, _Armoury


class MongoController:

    def __init__(self):
        self.items = _Items(default_database=self._mongo.get_default_database())
        self.users = _Users(default_database=self._mongo.get_default_database())
        self.armoury = _Armoury(default_database=self._mongo.get_default_database())
        self.bounties = _Bounties(default_database=self._mongo.get_default_database())

    @classmethod
    def create_connection(cls, con_str):
        cls._mongo = AsyncIOMotorClient(con_str)

    def get_client(self): return self._mongo

    def __enter__(self):
        return self

    def __exit__(self, exc_type, exc_val, exc_tb): ...
