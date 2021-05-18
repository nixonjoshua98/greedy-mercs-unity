
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

	inventory = mongo.db.inventories.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()

	armoury = mongo.db["userArmouryItems"].find({"userId": uid})

	bounties = mongo.db["userBounties"].find({"userId": uid})

	return {
		"player": {
			"username": info.get("username", "Rogue Mercenary"),
		},

		"inventory": inventory,

		"questsClaimed": quests.get("questsClaimed", dict()),

		"lifetimeStats": 	stats,
		"bountyShop": 		shop,

		"loot": 	inventory.get("loot", dict()),

		"armoury": list(armoury),
		"bounties": list(bounties)
	}
