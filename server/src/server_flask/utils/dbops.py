
from src.exts import mongo

from src import dbutils


def get_player_data(uid):
	"""
	Gets a users account data
	====================

	:param uid: The internal ID for the user we are updating

	:return:
		Returns the users data as a dict
	"""

	quests		= mongo.db.dailyQuests.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()
	stats 		= mongo.db.userStats.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()
	info  		= mongo.db.userInfo.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()

	items = mongo.db["userItems"].find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()

	armoury = mongo.db["userArmouryItems"].find({"userId": uid})

	bounties = mongo.db["userBounties"].find({"userId": uid})

	return {
		"player": {
			"username": info.get("username", "Rogue Mercenary"),
		},

		"inventory": {"items": items},

		"questsClaimed": quests.get("questsClaimed", dict()),

		"lifetimeStats": 	stats,
		"bountyShop": 		{"dailyPurchases": dbutils.bs.get_daily_purchases(uid)},

		"loot": 	items.get("loot", dict()),

		"armoury": list(armoury),
		"bounties": list(bounties),
	}
