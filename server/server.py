

from flask import Flask

from flask_pymongo import PyMongo

from src import utils

from src.views import (
	Login,
	StaticData,
	Prestige,
	BuyRelic, UpgradeRelic, ResetRelics,
	StartBounty, ClaimBounty,
	BuyWeapon
)

from src.classes.relic import Relic

app = Flask(__name__)

app.mongo = PyMongo()

app.staticdata = {
	"relics": 				utils.read_data_file("relics.json"),
	"bounties":				utils.read_data_file("bounties.json"),
	"weapons":				utils.read_data_file("weapons.json"),
	"characters": 			utils.read_data_file("characters.json"),
	"characterPassives":	utils.read_data_file("characterPassives.json"),
	}

app.objects = {
	"relics": {int(k): Relic.from_dict(r) for k, r in app.staticdata["relics"].items()}
}

app.mongo.init_app(app, uri="mongodb://localhost:27017/temp")

app.add_url_rule("/api/resetrelics", 	view_func=ResetRelics.as_view("resetrelics"), methods=["PUT"])

# === Bounties === #
app.add_url_rule("/api/bounty/start", view_func=StartBounty.as_view("startbounty"), methods=["PUT"])
app.add_url_rule("/api/bounty/claim", view_func=ClaimBounty.as_view("claimbounty"), methods=["PUT"])

# === Weapons === #
app.add_url_rule("/api/weapon/buy", view_func=BuyWeapon.as_view("buyweapon"), methods=["PUT"])


app.add_url_rule("/api/login", 			view_func=Login.as_view("login"), 					methods=["PUT"])
app.add_url_rule("/api/staticdata", 	view_func=StaticData.as_view("staticdata"), 		methods=["PUT"])
app.add_url_rule("/api/prestige", 		view_func=Prestige.as_view("prestige"), 			methods=["PUT"])
app.add_url_rule("/api/buyrelic", 		view_func=BuyRelic.as_view("buyrelic"), 			methods=["PUT"])
app.add_url_rule("/api/upgraderelic", 	view_func=UpgradeRelic.as_view("upgraderelic"),		methods=["PUT"])

app.run(host="0.0.0.0", debug=True, port=2122)
