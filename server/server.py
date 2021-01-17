

from flask import Flask

from flask_pymongo import PyMongo

from src import utils

from src.views import (
	Login,
	StaticData,
	Prestige,
	BuyLoot,
	UpgradeLoot,
	ResetAccount,
	ClaimBounty,
	BuyWeapon,
	ChangeUsername,
	PlayerLeaderboard
)

from src.staticobjects import Loot, Bounty, Weapon

app = Flask(__name__)

app.mongo = PyMongo()

app.staticdata = {
	"loot": 				utils.read_data_file("loot.json"),
	"bounties":				utils.read_data_file("bounties.json"),
	"weapons":				utils.read_data_file("weapons.json"),
	"characters": 			utils.read_data_file("characters.json"),
	"characterPassives":	utils.read_data_file("characterPassives.json"),
	}

app.objects = {
	"loot": 			{int(k): Loot.from_dict(r) for k, r in app.staticdata["loot"].items()},
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
app.add_url_rule("/api/loot/buy", view_func=BuyLoot.as_view("buyitem"), methods=["PUT"])
app.add_url_rule("/api/loot/upgrade", view_func=UpgradeLoot.as_view("upgradeitem"), methods=["PUT"])

# === Player === #
app.add_url_rule("/api/user/changeusername", view_func=ChangeUsername.as_view("changeusername"), methods=["PUT"])

# === Leaderboard === #
app.add_url_rule("/api/leaderboard/player", view_func=PlayerLeaderboard.as_view("playerleaderboard"), methods=["PUT"])


app.add_url_rule("/api/login", 			view_func=Login.as_view("login"), 					methods=["PUT"])
app.add_url_rule("/api/staticdata", 	view_func=StaticData.as_view("staticdata"), 		methods=["PUT"])
app.add_url_rule("/api/prestige", 		view_func=Prestige.as_view("prestige"), 			methods=["PUT"])

app.run(host="0.0.0.0", debug=True, port=2122)
