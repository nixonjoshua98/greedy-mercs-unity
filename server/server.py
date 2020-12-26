

from flask import Flask

from flask_pymongo import PyMongo

from src.views import Login, StaticData, Prestige, BuyRelic


app = Flask(__name__)

mongo = PyMongo()

mongo.init_app(app, uri="mongodb://localhost:27017/temp")

app.add_url_rule("/api/login", 		view_func=Login.as_view("login", mongo), 		methods=["PUT"])
app.add_url_rule("/api/staticdata", view_func=StaticData.as_view("staticdata"), 	methods=["PUT"])
app.add_url_rule("/api/prestige", 	view_func=Prestige.as_view("prestige", mongo), 	methods=["PUT"])
app.add_url_rule("/api/buyrelic", 	view_func=BuyRelic.as_view("buyrelic", mongo), 	methods=["PUT"])

app.run(host="0.0.0.0", debug=True, port=2122)