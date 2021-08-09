
from .client import MotorClient


def create_client(con_str) -> MotorClient:
    return MotorClient(con_str)
