from motor.motor_asyncio import AsyncIOMotorClient


class MotorClient:
    def __init__(self, con_str: str):
        # Avoids accidental database creations
        self._client = AsyncIOMotorClient(con_str)

    @property
    def database(self):
        return self._client["g0"]
