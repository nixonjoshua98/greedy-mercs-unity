import os
import time

from flask import Flask

from flask_pymongo import PyMongo

from configparser import ConfigParser

from src import utils

from src.safescheduler import SafeScheduler

from src.staticobjects import Loot, Bounty, Weapon

from src.views.baseview import BaseView


class FlaskApplication(Flask):
	def __init__(self, *args, **kwargs):
		super(FlaskApplication, self).__init__(*args, **kwargs)

		self.staticdata = self.load_static()
		self.objects = self.create_objects()

		self.scheduler = SafeScheduler()

		self.scheduler.run()

	def add_url_rule(self, rule, endpoint=None, view_func=None, provide_automatic_options=None, **options):
		super(FlaskApplication, self).add_url_rule(rule, endpoint, view_func, provide_automatic_options, **options)

		if (view := self.view_functions.get(endpoint)) is not None:
			if hasattr(view, "view_class") and issubclass(view.view_class, BaseView):
				view.view_class.on_startup()

	def full_dispatch_request(self):
		now = time.time()

		val = super(FlaskApplication, self).full_dispatch_request()

		print("Dispatched request in ", time.time() - now, "s", sep="")

		return val

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


def create_app():
	config = ConfigParser()

	config.read("config.ini")

	app = FlaskApplication(__name__)

	app.mongo = PyMongo()

	app.mongo.init_app(app, uri=config.get("debug" if os.getenv("DEBUG", 0) == 0 else "server", "MONGO_URI"))

	return app
