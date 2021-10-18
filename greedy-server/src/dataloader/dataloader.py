from motor.motor_asyncio import AsyncIOMotorClient


class DataLoader:
    def __init__(self):
        self.users = _Users(self._mongo.get_default_database())

    @classmethod
    def create_client(cls, con_str):
        cls._mongo = AsyncIOMotorClient(con_str)


class _Users:
    def __init__(self, default_database):
        self.collection = default_database["userLogins"]

    async def get_user(self, device_id: str):
        return await self.collection.find_one({"deviceId": device_id})

    async def insert_new_user(self, data: dict):
        r = await self.collection.insert_one(data)

        return r.inserted_id
