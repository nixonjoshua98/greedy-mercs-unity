
from motor.motor_asyncio import AsyncIOMotorClient

from src.classes.mixins import ContextLoggerMixin

from .children import _Users, _Bounties, _Items


class MongoController(ContextLoggerMixin):

    def __init__(self):
        self.items = _Items(default_database=self._mongo.get_default_database())
        self.users = _Users(default_database=self._mongo.get_default_database())
        self.bounties = _Bounties(default_database=self._mongo.get_default_database())

    @classmethod
    def create_connection(cls, con_str):
        cls._mongo = AsyncIOMotorClient(con_str)

    def get_client(self): return self._mongo
