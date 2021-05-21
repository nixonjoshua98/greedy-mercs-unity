
from pymongo import MongoClient

from pymongo.database import Database

client = None

db: Database = None


def create_mongo():
	import os

	global client, db

	client = MongoClient(os.getenv("MONGO_URI") or "mongodb://localhost:27017/")

	db = client["g0"]


create_mongo()
