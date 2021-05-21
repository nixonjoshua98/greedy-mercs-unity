
from src.exts import mongo

from pymongo import ReturnDocument


def update_items(uid, *, inc: dict):
	return mongo.db["userItems"].find_one_and_update({"userId": uid}, {"$inc": inc}, upsert=True, return_document=ReturnDocument.AFTER)
