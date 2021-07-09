from flask import Flask

from flask.json import JSONEncoder

from .views import *

from bson import ObjectId


class MyJsonEncoder(JSONEncoder):
	def default(self, o):
		if isinstance(o, ObjectId):
			return str(o)

		return super(MyJsonEncoder, self).default(o)


def create_app():
	app = Flask(__name__)

	app.json_encoder = MyJsonEncoder

	app.add_url_rule("/api/bounty", view_func=BountyView.as_view("bounty"), methods=["PUT"])
	app.add_url_rule("/api/armoury", view_func=ArmouryView.as_view("armoury"), methods=["PUT"])
	app.add_url_rule("/api/artefacts", view_func=ArtefactsView.as_view("artefacts"), methods=["PUT"])
	app.add_url_rule("/api/bountyshop", view_func=BountyShopView.as_view("bountyshop"), methods=["PUT"])

	# === Player (User) === #
	app.add_url_rule("/api/user/login", view_func=PlayerLogin.as_view("playerlogin"), methods=["PUT"])
	app.add_url_rule("/api/user/changeusername", view_func=ChangeUsername.as_view("changeusername"), methods=["PUT"])

	# === Data === #
	app.add_url_rule("/api/gamedata", view_func=ServerData.as_view("gamedata"), methods=["GET"])

	app.add_url_rule("/api/prestige", view_func=Prestige.as_view("prestige"), methods=["PUT"])

	return app
