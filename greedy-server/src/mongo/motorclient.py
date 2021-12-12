from motor.motor_asyncio import AsyncIOMotorClient


class MotorClient:
    def __init__(self, con_str: str):
        self._client = AsyncIOMotorClient(con_str)

    @property
    def database(self):
        return self._client["g0"]
