

import src

from src.views import (
	Login, StaticData, Prestige, BuyLoot, UpgradeLoot,
	ResetAccount, ClaimBounty, BuyWeapon, ChangeUsername, PlayerLeaderboard
)

DEBUG = False

app = src.create_app(debug=DEBUG)


# === Temp === #
app.add_url_rule("/api/resetrelics", view_func=ResetAccount.as_view("resetrelics"), methods=["PUT"])

# === Bounties === #
app.add_url_rule("/api/bounty/claim", view_func=ClaimBounty.as_view("claimbounty"), methods=["PUT"])

# === Weapons === #
app.add_url_rule("/api/weapon/buy", view_func=BuyWeapon.as_view("buyweapon"), methods=["PUT"])

# === Loot Items === #
app.add_url_rule("/api/loot/buy", 		view_func=BuyLoot.as_view("buyitem"), 			methods=["PUT"])
app.add_url_rule("/api/loot/upgrade", 	view_func=UpgradeLoot.as_view("upgradeitem"), 	methods=["PUT"])

# === Player === #
app.add_url_rule("/api/user/changeusername", view_func=ChangeUsername.as_view("changeusername"), methods=["PUT"])

# === Leaderboard === #
app.add_url_rule("/api/leaderboard/player", view_func=PlayerLeaderboard.as_view("playerleaderboard"), methods=["PUT"])


app.add_url_rule("/api/login", 			view_func=Login.as_view("login"), 					methods=["PUT"])
app.add_url_rule("/api/staticdata", 	view_func=StaticData.as_view("staticdata"), 		methods=["PUT"])
app.add_url_rule("/api/prestige", 		view_func=Prestige.as_view("prestige"), 			methods=["PUT"])

if __name__ == "__main__":
	app.run(host="0.0.0.0", debug=DEBUG, port=2122)