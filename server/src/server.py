from flask_pymongo import PyMongo


mongo = PyMongo()


def run():
	from flask import Flask

	from src.views import login, staticdata

	app = Flask(__name__)

	mongo.init_app(app, uri="mongodb://localhost:27017/temp")

	app.add_url_rule("/api/login", 		view_func=login.login, 				methods=["PUT"])
	app.add_url_rule("/api/staticdata", view_func=staticdata.staticdata, 	methods=["PUT"])

	app.run(host="0.0.0.0", debug=True, port=2122)