import typing

from .client import DataLoader
from .mongo import MongoController

__client: typing.Union[None, DataLoader] = None


def create_client(con_str) -> DataLoader:
    global __client

    __client = DataLoader(con_str)

    MongoController.create_connection(con_str)

    return __client


def get_loader():
    if __client is None:
        raise RuntimeError("DataLoader is None")

    return __client