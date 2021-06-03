
from src.common import mongo


def get_armoury(uid) -> list:
	return list(mongo.db["userArmouryItems"].find({"userId": uid}))


def update_item(uid: int, iid: int, inc: dict) -> list:

	mongo.db["userArmouryItems"].update_one({"userId": uid, "itemId": iid}, {"$inc": inc}, upsert=True)

	return get_armoury(uid)
