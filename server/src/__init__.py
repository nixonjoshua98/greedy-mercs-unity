import os

from flask_pymongo import PyMongo

from configparser import ConfigParser

from src import utils

from src.staticobjects import Loot, Bounty, Weapon

from src.classes import FlaskApplication


def create_app():
	config = ConfigParser()

	config.read("config.ini")

	app = FlaskApplication(__name__)

	app.mongo = PyMongo()

	app.mongo.init_app(app, uri=config.get("debug" if os.getenv("DEBUG") == "1" else "production", "MONGO_URI"))

	return app
