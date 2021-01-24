from flask import current_app as app

from src import formulas


def add_bounty_prestige_levels(userid, stage):
	"""
	Updates a users bounty levels based upon their prestige stage
	====================

	:param userid: The internal ID for the user we are updating
	:param stage: The stage at which the user prestiged at

	:return:
		Returns the result of the query
	"""

	levels = (app.mongo.db.userBounties.find_one({"userId": userid}) or dict()).get("bountyLevels", dict())

	levels_earned = formulas.stage_bounty_levels(stage, levels)

	query = {f"bountyLevels.{key}": level for key, level in levels_earned.items()}

	if query:
		app.mongo.db.userBounties.update_one({"userId": userid}, {"$inc": query})


def get_player_data(uid):
	"""
	Gets a users account data
	====================

	:param uid: The internal ID for the user we are updating

	:return:
		Returns the users data as a dict
	"""

	shop  = app.mongo.db.bountyShop.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()
	items = app.mongo.db.userItems.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()
	stats = app.mongo.db.userStats.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()

	bounties = app.mongo.db.userBounties.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()

	return {
		"player": {
			"username": 		stats.get("username", "Rogue Mercenary"),
			"maxPrestigeStage": stats.get("maxPrestigeStage", 0),

			"bountyPoints": 	items.get("bountyPoints", 0),
			"prestigePoints": 	items.get("prestigePoints", 0),
			"gems": 			items.get("gems", 0),
			"weaponPoints": 	items.get("weaponPoints", 0)
		},

		"bountyShop": shop,

		"bounties": bounties,
		"weapons": 	items.get("weapons", dict()),
		"loot": 	items.get("loot", dict())
	}