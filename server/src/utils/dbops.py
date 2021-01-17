from flask import current_app as app

from pymongo import MongoClient

from bson import ObjectId


def update_max_prestige_stage(mongo: MongoClient, userid: ObjectId, stage: int):
	"""
	Updates a users max prestge stage
	====================

	:param mongo: The Mongo object, attached to the Flask application
	:param userid: The internal ID for the user we are updating
	:param stage: The stage at which the user prestiged at

	:return:
		Returns the result of the query
	"""

	return mongo.db.userStats.update_one(
		{
			"userId": userid,
			"$or": [
				{"maxPrestigeStage": {"$lt": stage}},
				{"maxPrestigeStage": {"$exists": False}}
			]
		},
		{"$set": {"maxPrestigeStage": stage}},
		upsert=True
	)


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


def get_player_data(mongo, uid):
	"""
	Gets a users account data
	====================

	:param mongo: The Mongo object, attached to the Flask application
	:param uid: The internal ID for the user we are updating

	:return:
		Returns the users data as a dict
	"""

	items = mongo.db.userItems.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()
	stats = mongo.db.userStats.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()

	bounties = mongo.db.userBounties.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()

	return {
		"player": {
			"bountyPoints": 	items.get("bountyPoints", 0),
			"maxPrestigeStage": stats.get("maxPrestigeStage", 0),
			"prestigePoints": 	str(items.get("prestigePoints", 0)),
			"username": 		stats.get("username", "Rogue Mercenary"),
		},

		"bounties": bounties,
		"weapons": 	items.get("weapons", dict()),
		"loot": 	items.get("loot", dict())
	}