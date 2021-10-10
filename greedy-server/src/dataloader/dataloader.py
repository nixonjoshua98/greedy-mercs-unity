from motor.motor_asyncio import AsyncIOMotorClient


class DataLoader:
    def __init__(self):
        db = self._mongo.get_default_database()

        self.users = _Users(db)

    @classmethod
    def create_client(cls, con_str):
        cls._mongo = AsyncIOMotorClient(con_str)

    def __enter__(self):
        return self

    def __exit__(self, exc_type, exc_val, exc_tb): ...


class _Users:
    def __init__(self, default_database):
        self.collection = default_database["userLogins"]

    async def get_user(self, device_id: str):
        return await self.collection.find_one({"deviceId": device_id})

    async def insert_new_user(self, data: dict):
        r = await self.collection.insert_one(data)

        return r.inserted_id
