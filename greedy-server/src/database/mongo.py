import functools as ft
from typing import Union

from pymongo import MongoClient

from .common import DEFAULT_DATABASE

__client: Union[MongoClient, None] = None


@ft.cache
def get_client():
    global __client

    __client = MongoClient("mongodb://localhost:27017/g0")

    return __client


def get_collection(col):
    return get_client()[DEFAULT_DATABASE][col]
