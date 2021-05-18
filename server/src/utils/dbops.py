
from pymongo import ReturnDocument

from src.exts import mongo


def get_player_data(uid):
	"""
	Gets a users account data
	====================

	:param uid: The internal ID for the user we are updating

	:return:
		Returns the users data as a dict
	"""

	shop  		= mongo.db.dailyResetPurchases.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()
	quests		= mongo.db.dailyQuests.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()
	stats 		= mongo.db.userStats.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()
	info  		= mongo.db.userInfo.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()
	bounties 	= mongo.db.userBounties.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()

	inventory = mongo.db.inventories.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()

	armoury = mongo.db["userArmouryItems"].find_one({"userId": uid}) or dict()

	return {
		"player": {
			"username": info.get("username", "Rogue Mercenary"),
		},

		"inventory": inventory,

		"questsClaimed": quests.get("questsClaimed", dict()),

		"lifetimeStats": 	stats,
		"bountyShop": 		shop,
		"bounties": 		bounties,

		"loot": 	inventory.get("loot", dict()),

		"armoury": 	armoury.get("items", {})
	}


def update_armoury_item(uid, iid, *, points=0, levels=0, owned=0, evolevels=0):
	"""
	Update an armoury item, as well as having the option of incrementing the users armoury points
	====================

	:return:
		Return the users inventory after the query has executed
	"""

	return mongo.db.inventories.find_one_and_update(
		{"userId": uid},
		{
			"$inc": {
				"armouryPoints": points,

				f"armoury.{iid}.level": levels,
				f"armoury.{iid}.owned": owned,
				f"armoury.{iid}.evoLevel": evolevels
			}
		},
		upsert=True,
		return_document=ReturnDocument.AFTER
	)