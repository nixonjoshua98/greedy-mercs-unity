from pymongo import MongoClient, ReturnDocument

from flask import current_app as app

from bson import ObjectId


def get_bounty_shop_and_update(userid):
	"""
	Gets a users bounty shop entry, and updates it if it is out of date (from a previous reset)
	====================

	:param userid: The internal ID for the user we are updating

	:return:
		Return a the document (dict) from the database
	"""
	shop = app.mongo.db.userBountyShop.find_one({"userId": userid}, {"_id": 0, "userId": 0}) or dict()

	shop_needs_updating = (last_reset := shop.get("lastReset")) is not None and app.last_daily_reset > last_reset

	if last_reset is None or shop_needs_updating:
		shop = app.mongo.db.userBountyShop.find_one_and_update(
			{
				"userId": userid
			},

			{
				"$set": {
					"lastReset": app.last_daily_reset,
					"itemsBought": {"0": 0}
				}
			},

			projection={'_id': False, 'userId': False},
			upsert=True,
			return_document=ReturnDocument.AFTER
		)

	return shop


def update_max_prestige_stage(mongo: MongoClient, userid: ObjectId, stage: int) -> None:
	"""
	Updates a users max prestge stage
	====================

	:param mongo: The Mongo object, attached to the Flask application
	:param userid: The internal ID for the user we are updating
	:param stage: The stage at which the user prestiged at
	"""

	result = mongo.db.userStats.find_one({"userId": userid})

	if result is None or result.get("maxPrestigeStage", 0) < stage:
		mongo.db.userStats.update_one({"userId": userid}, {"$set": {"maxPrestigeStage": stage}}, upsert=True)


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

	bounty_levels = prestige_bounty_levels_earned()

	return app.mongo.db.userBounties.update_one(
		{"userId": userid},
		{"$inc": {f"bountyLevels.{key}": level for key, level in bounty_levels.items()}}
	)


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
		},

		"userBountyShop": bounty_shop,

		"bounties": bounties,
		"weapons": 	items.get("weapons", dict()),
		"loot": 	items.get("loot", dict())
	}