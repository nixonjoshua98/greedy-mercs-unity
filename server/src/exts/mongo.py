
from pymongo import MongoClient, ReturnDocument

from pymongo.database import Database

client = None

db: Database = None


def update_inventory(uid, *, inc_: dict):
	return db["inventories"].find_one_and_update(
		{"userId": uid},
		{"$inc": inc_},
		upsert=True,
		return_document=ReturnDocument.AFTER
	)


def create_mongo():
	import os

	global client, db

	client = MongoClient(os.getenv("MONGO_URI") or "mongodb://localhost:27017/")

	db = client["_greedy"]
