
from typing import Union

from src.common import mongo


def update_one(search: dict, update: dict, *, upsert: bool) -> bool:
    result = mongo.db["userArmouryItems"].update_one(search, update, upsert=upsert)

    return result.modified_count > 0


def find(search) -> list[dict]:
    return list(mongo.db["userArmouryItems"].find(search))


def find_one(search) -> Union[dict, None]:
    return mongo.db["userArmouryItems"].find_one(search)
