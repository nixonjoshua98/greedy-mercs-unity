
from motor.motor_asyncio import AsyncIOMotorClient


def create_client(con_str) -> AsyncIOMotorClient:
    return AsyncIOMotorClient(con_str)

