

from flask import Flask

from flask_pymongo import PyMongo

from src import utils

from src.views import (
	Login,
	StaticData,
	Prestige,
	BuyPrestigeItem, UpgradePrestigeItem,
	ResetAccount,
	ClaimBounty,
	BuyWeapon
)

from src.staticobjects import PrestigeItem, Bounty, Weapon

app = Flask(__name__)

app.mongo = PyMongo()

app.staticdata = {
	"prestigeItems": 		utils.read_data_file("prestigeitems.json"),
	"bounties":				utils.read_data_file("bounties.json"),
	"weapons":				utils.read_data_file("weapons.json"),
	"characters": 			utils.read_data_file("characters.json"),
	"characterPassives":	utils.read_data_file("characterPassives.json"),
	}

app.objects = {
	"prestigeItems": 	{int(k): PrestigeItem.from_dict(r) for k, r in app.staticdata["prestigeItems"].items()},
	"bounties": 		{int(k): Bounty.from_dict(r) for k, r in app.staticdata["bounties"].items()},
	"weapons": 			{int(k): Weapon.from_dict(r) for k, r in app.staticdata["weapons"].items()}
}

app.mongo.init_app(app, uri="mongodb://localhost:27017/temp")

# === Temp === #
app.add_url_rule("/api/resetrelics", view_func=ResetAccount.as_view("resetrelics"), methods=["PUT"])

# === Bounties === #
app.add_url_rule("/api/bounty/claim", view_func=ClaimBounty.as_view("claimbounty"), methods=["PUT"])

# === Weapons === #
app.add_url_rule("/api/weapon/buy", view_func=BuyWeapon.as_view("buyweapon"), methods=["PUT"])

# === Prestige Items === #
app.add_url_rule("/api/prestigeitems/buy", 		view_func=BuyPrestigeItem.as_view("buyrelic"), 			methods=["PUT"])
app.add_url_rule("/api/prestigeitems/upgrade", 	view_func=UpgradePrestigeItem.as_view("upgraderelic"), 	methods=["PUT"])


app.add_url_rule("/api/login", 			view_func=Login.as_view("login"), 					methods=["PUT"])
app.add_url_rule("/api/staticdata", 	view_func=StaticData.as_view("staticdata"), 		methods=["PUT"])
app.add_url_rule("/api/prestige", 		view_func=Prestige.as_view("prestige"), 			methods=["PUT"])

app.run(host="0.0.0.0", debug=True, port=2122)
