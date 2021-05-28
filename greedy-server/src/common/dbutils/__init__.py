
from . import bs, armoury

from src.common import mongo

from pymongo import ReturnDocument

import datetime as dt


def next_daily_reset(): return last_daily_reset() + dt.timedelta(days=1)


def last_daily_reset():
	reset_time = (now := dt.datetime.utcnow()).replace(hour=20, minute=0, second=0, microsecond=0)

	return reset_time - dt.timedelta(days=1) if now <= reset_time else reset_time


class inventory:

	@staticmethod
	def update_items(uid, *, inc: dict):
		return mongo.db["userItems"].find_one_and_update({"userId": uid}, {"$inc": inc}, upsert=True, return_document=ReturnDocument.AFTER)

	@staticmethod
	def get_items(uid):
		return mongo.db["userItems"].find_one({"userId": uid}) or dict()


def get_player_data(uid):
	quests		= mongo.db.dailyQuests.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()
	stats 		= mongo.db.userStats.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()
	info  		= mongo.db.userInfo.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()

	bounties = mongo.db["userBounties"].find({"userId": uid})

	return {
		"player": {
			"username": info.get("username", "Rogue Mercenary"),
		},

		"inventory": {"items": inventory.get_items(uid)},

		"questsClaimed": quests.get("questsClaimed", dict()),

		"lifetimeStats": stats,
		"bountyShop": {"dailyPurchases": bs.get_daily_purchases(uid)},

		"loot": mongo.db["inventories"].find_one({"userId": uid}).get("loot", dict()),

		"armoury": armoury.get_armoury(uid),
		"bounties": list(bounties),
	}
