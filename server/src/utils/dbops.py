from pymongo import MongoClient

from bson import ObjectId

import datetime as dt


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
		{"userId": userid, "maxPrestigeStage": {"$lt": stage}},
		{"$set": {"maxPrestigeStage": stage}},
		upsert=True
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
	bounties = mongo.db.userBounties.find_one({"userId": uid}, {"_id": 0, "userId": 0})

	if bounties is None:
		now = dt.datetime.utcnow()

		mongo.db.userBounties.update_one({"userId": uid}, {"$set": {"lastClaimTime": now}}, upsert=True)

		bounties = {"lastClaimTime": now}

	return {
		"weapons": items.get("weapons", dict()),
		"relics": items.get("relics", dict()),

		"bounties": bounties,

		"bountyPoints": items.get("bountyPoints", 0),
		"prestigePoints": str(items.get("prestigePoints", 0))
	}