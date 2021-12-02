from motor.motor_asyncio import AsyncIOMotorClient


class MotorClient(AsyncIOMotorClient):

    @property
    def db(self):
        return self["g0"]
