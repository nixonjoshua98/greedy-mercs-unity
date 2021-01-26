from pymongo import ReturnDocument

from src.exts import mongo
from src.classes.gamedata import GameData


def get_player_data(uid):
	"""
	Gets a users account data
	====================

	:param uid: The internal ID for the user we are updating

	:return:
		Returns the users data as a dict
	"""

	shop  = mongo.db.bountyShop.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()
	items = mongo.db.userItems.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()
	stats = mongo.db.userStats.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()

	bounties = mongo.db.userBounties.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()

	if shop is None or shop.get("lastReset") is None:
		shop = mongo.db.bountyShop.find_one_and_update(
			{
				"userId": uid
			},
			{
				"$set": {"lastReset": GameData.last_daily_reset}
			},

			upsert=True,
			projection={"_id": 0, "userId": 0},
			return_document=ReturnDocument.AFTER
		)

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