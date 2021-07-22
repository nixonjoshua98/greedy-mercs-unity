from pymongo import ReturnDocument

from src.common import mongo


class Items:

    IRON_INGOTS = "ironIngots"

    @staticmethod
    def find_one(search) -> dict:
        return mongo.db["userItems"].find_one(search) or dict()

    @staticmethod
    def update_one(search: dict, update: dict, *, upsert: bool = True) -> bool:
        result = mongo.db["userItems"].update_one(search, update, upsert=upsert)

        return result.modified_count > 0

    @staticmethod
    def find_and_update_one(search: dict, update: dict) -> dict:
        return mongo.db["userItems"].find_one_and_update(
            search, update, upsert=True, return_document=ReturnDocument.AFTER
        )


def update_items(uid, *, inc: dict):
    mongo.db["userItems"].update_one({"userId": uid}, {"$inc": inc}, upsert=True)


def get_items(uid):
    return mongo.db["userItems"].find_one({"userId": uid}) or dict()
