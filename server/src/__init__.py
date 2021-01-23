import os

from pymongo import MongoClient

from src import utils

from src.classes import FlaskApplication


def create_app():
	app = FlaskApplication(__name__)

	if (uri := os.getenv("MONGO_URI")) is None:
		uri = "mongodb://localhost:27017/greedymercs"

	app.mongo = MongoClient(uri)

	return app
