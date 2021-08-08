from pymongo import MongoClient

from .items import Items as _Items


DATABASE_NAME = "g0"


class MongoController:
    def __init__(self):
        self._client = None

        self.db = None

        self.items = None

    def connect(self):
        self._client = MongoClient("mongodb://localhost:27017/g0")

        self.db = self._client.get_database(DATABASE_NAME)

        self._setup_controllers()

    def _setup_controllers(self):
        self.items = _Items(self)


mongo = MongoController()
mongo.connect()