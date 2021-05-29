from flask import Flask

from .views import *


def create_app():
	app = Flask(__name__)

	app.add_url_rule("/api/bounty", view_func=Bounty.as_view("bounty"), methods=["PUT"])
	app.add_url_rule("/api/armoury", view_func=Armoury.as_view("armoury"), methods=["PUT"])
	app.add_url_rule("/api/bountyshop", view_func=BountyShop.as_view("bountyshop"), methods=["PUT"])
	app.add_url_rule("/api/artefacts", view_func=ArtefactsView.as_view("artefacts"), methods=["PUT"])

	# === Player (User) === #
	app.add_url_rule("/api/user/login", view_func=PlayerLogin.as_view("playerlogin"), methods=["PUT"])
	app.add_url_rule("/api/user/changeusername", view_func=ChangeUsername.as_view("changeusername"), methods=["PUT"])

	# === Leaderboard === #
	app.add_url_rule("/api/leaderboard/player", view_func=PlayerLeaderboard.as_view("playerleaderboard"), methods=["GET"])

	# === Data === #
	app.add_url_rule("/api/gamedata", view_func=ServerData.as_view("gamedata"), methods=["GET"])

	app.add_url_rule("/api/prestige", view_func=Prestige.as_view("prestige"), methods=["PUT"])

	return app
