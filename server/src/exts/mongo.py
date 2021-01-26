

client = None

db = None


def create_mongo():
	import os

	from pymongo import MongoClient

	global client, db

	client = MongoClient(os.getenv("MONGO_URI") or "mongodb://localhost:27017/greedymercs")

	db = client.get_default_database()
