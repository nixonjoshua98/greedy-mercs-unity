from pymongo import ReturnDocument

from flask import current_app as app

from bson import ObjectId

from src import formulas


def add_prestige_points(userid, *, stage: int):
	items = app.mongo.db.userItems.find_one({"userId": userid}) or dict()

	points = formulas.calc_stage_prestige_points(stage, items.get("loot", dict()))

	pp = int(items.get("prestigePoints", 0)) + points

	app.mongo.db.userItems.update_one({"userId": userid}, {"$set": {"prestigePoints": str(pp)}}, upsert=True)


def get_bounty_shop_and_update(userid):
	"""
	Gets a users bounty shop entry, and updates it if it is out of date (from a previous reset)
	====================

	:param userid: The internal ID for the user we are updating

	:return:
		Return a the document (dict) from the database
	"""
	shop = app.mongo.db.userBountyShop.find_one({"userId": userid}, {"_id": 0, "userId": 0}) or dict()

	shop_needs_updating = (last_reset := shop.get("lastPurchaseReset")) is not None and app.last_daily_reset > last_reset

	if last_reset is None or shop_needs_updating:
		shop = app.mongo.db.userBountyShop.find_one_and_update(
			{
				"userId": userid
			},

			{
				"$set": {"lastPurchaseReset": app.last_daily_reset},
				"$unset": {"itemsBought": None}
			},

			projection={'_id': False, 'userId': False},
			upsert=True,
			return_document=ReturnDocument.AFTER
		)

	return shop


def update_max_prestige_stage(userid: ObjectId, stage: int) -> None:
	"""
	Updates a users max prestge stage
	====================

	:param userid: The internal ID for the user we are updating
	:param stage: The stage at which the user prestiged at
	"""

	result = app.mongo.db.userStats.find_one({"userId": userid})

	if result is None or result.get("maxPrestigeStage", 0) < stage:
		app.mongo.db.userStats.update_one({"userId": userid}, {"$set": {"maxPrestigeStage": stage}}, upsert=True)


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

	def prestige_bounty_levels_earned():
		new_levels = dict()

		for key, bounty in app.objects["bounties"].items():
			level = levels.get(str(key), 0)

			if stage > bounty.unlock_stage and level + 1 <= bounty.max_level:
				new_levels[key] = 1

		return new_levels

	query = {f"bountyLevels.{key}": level for key, level in prestige_bounty_levels_earned().items()}

	if query:
		app.mongo.db.userBounties.update_one({"userId": userid}, {"$inc": query})


def get_player_data(userid):
	"""
	Gets a users account data
	====================

	:param userid: The internal ID for the user we are updating

	:return:
		Returns the users data as a dict
	"""

	bounty_shop = get_bounty_shop_and_update(userid)

	items = app.mongo.db.userItems.find_one({"userId": userid}, {"_id": 0, "userId": 0}) or dict()
	stats = app.mongo.db.userStats.find_one({"userId": userid}, {"_id": 0, "userId": 0}) or dict()

	bounties = app.mongo.db.userBounties.find_one({"userId": userid}, {"_id": 0, "userId": 0}) or dict()

	return {
		"player": {
			"bountyPoints": 	items.get("bountyPoints", 0),
			"maxPrestigeStage": stats.get("maxPrestigeStage", 0),
			"prestigePoints": 	str(items.get("prestigePoints", 0)),
			"username": 		stats.get("username", "Rogue Mercenary"),
			"gems": 			items.get("gems", 0)
		},

		"userBountyShop": bounty_shop,

		"bounties": bounties,
		"weapons": 	items.get("weapons", dict()),
		"loot": 	items.get("loot", dict())
	}