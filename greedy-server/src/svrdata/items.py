from src.common import mongo


def update_items(uid, *, inc: dict):
    mongo.db["userItems"].update_one({"userId": uid}, {"$inc": inc}, upsert=True)


def get_items(uid):
    return mongo.db["userItems"].find_one({"userId": uid}) or dict()
