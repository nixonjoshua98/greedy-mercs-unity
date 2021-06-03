from src.common import mongo

from pymongo import ReturnDocument


def update_items(uid, *, inc: dict):
	return mongo.db["userItems"].find_one_and_update({"userId": uid}, {"$inc": inc}, upsert=True, return_document=ReturnDocument.AFTER)


def get_items(uid):
	return mongo.db["userItems"].find_one({"userId": uid}) or dict()
