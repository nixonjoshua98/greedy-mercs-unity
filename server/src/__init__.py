import os

from flask_pymongo import PyMongo

from src import utils

from src.staticobjects import Loot, Bounty, Weapon

from src.classes import FlaskApplication


class PyM(PyMongo):
	def init_app(self, app, uri=None, *args, **kwargs):
		from flask_pymongo import BSONObjectIdConverter
		from pymongo import uri_parser
		from flask_pymongo.wrappers import MongoClient

		if uri is None:
			uri = app.config.get("MONGO_URI", None)
		if uri is not None:
			args = tuple([uri] + list(args))
		else:
			raise ValueError(
				"You must specify a URI or set the MONGO_URI Flask config variable",
			)

		parsed_uri = uri_parser.parse_uri(uri)

		print("Parsed URI:", parsed_uri)

		database_name = parsed_uri["database"]

		# Try to delay connecting, in case the app is loaded before forking, per
		# http://api.mongodb.com/python/current/faq.html#is-pymongo-fork-safe
		kwargs.setdefault("connect", False)

		self.cx = MongoClient(*args, **kwargs)
		if database_name:
			self.db = self.cx[database_name]

		app.url_map.converters["ObjectId"] = BSONObjectIdConverter


def create_app():
	app = FlaskApplication(__name__)

	app.mongo = PyM()

	if (uri := os.getenv("MONGO_URI")) is None:
		uri = "mongodb://localhost:27017/greedymercs"

	app.mongo.init_app(app, uri=uri)

	return app
