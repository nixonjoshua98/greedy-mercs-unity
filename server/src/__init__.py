import os

from flask_pymongo import PyMongo

from src import utils

from src.staticobjects import Loot, Bounty, Weapon

from src.classes import FlaskApplication


def create_app():
	app = FlaskApplication(__name__)

	app.mongo = PyMongo()

	if (uri := os.getenv("MONGO_URI")) is None:
		uri = "mongodb://localhost:27017/greedymercs"

	app.mongo.init_app(app, uri=uri)

	return app
