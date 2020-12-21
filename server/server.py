

from flask import Flask

from flask_pymongo import PyMongo

from src.views.staticdata import StaticData


app = Flask(__name__)

mongo = PyMongo()

mongo.init_app(app, uri="mongodb://localhost:27017/temp")

app.add_url_rule("/api/staticdata", view_func=StaticData.as_view("staticdata"), methods=["PUT"])

app.run(host="0.0.0.0", debug=True, port=2122)