from motor.motor_asyncio import AsyncIOMotorClient


class MotorClient:
    def __init__(self, con_str: str):
        self._client = AsyncIOMotorClient(con_str)

        self.database = self._client["g0"]
