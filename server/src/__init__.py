from flask import Flask

from flask_pymongo import PyMongo

from configparser import ConfigParser

from src import utils

from src.staticobjects import Loot, Bounty, Weapon


class FlaskApplication(Flask):
	def __init__(self, *args, **kwargs):
		super(FlaskApplication, self).__init__(*args, **kwargs)

		self.staticdata = self.load_static()
		self.objects = self.create_objects()

	def load_static(self) -> dict:
		return {
			"loot": utils.read_data_file("loot.json"),
			"bounties": utils.read_data_file("bounties.json"),
			"weapons": utils.read_data_file("weapons.json"),
			"characters": utils.read_data_file("characters.json"),
			"characterPassives": utils.read_data_file("characterPassives.json"),
		}

	def create_objects(self):
		return {
			"loot": {int(k): Loot.from_dict(r) for k, r in self.staticdata["loot"].items()},
			"bounties": {int(k): Bounty.from_dict(r) for k, r in self.staticdata["bounties"].items()},
			"weapons": {int(k): Weapon.from_dict(r) for k, r in self.staticdata["weapons"].items()}
		}


def create_app(*, debug: bool):
	config = ConfigParser()

	config.read("config.ini")

	app = FlaskApplication(__name__)

	app.mongo = PyMongo()

	app.mongo.init_app(app, uri=config.get("debug" if debug else "server", "MONGO_URI"))

	return app
