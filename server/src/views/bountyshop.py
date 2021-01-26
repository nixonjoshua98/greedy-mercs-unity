import random

from flask import request, Response

from flask.views import View

from src import utils, checks, formulas

from src.classes.gamedata import GameData
from src.enums import BountyShopItem
from src.exts import mongo


class BountyShopRefresh(View):

	@checks.login_check
	def dispatch_request(self, uid):
		shop = mongo.db.bountyShop.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()

		return Response(utils.compress(shop), status=200)


class BountyShop(View):

	@checks.login_check
	def dispatch_request(self, uid):

		data = utils.decompress(request.data)

		# - Load data from database
		shop 	= mongo.db.bountyShop.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()
		items 	= mongo.db.userItems.find_one({"userId": uid}) or dict()

		item = data["itemId"]

		bounty_points 	= items.get("bountyPoints", 0)
		num_bought 		= shop.get("itemsBought", dict()).get(str(item), 0)

		item_data = GameData.get_item("bountyShopItems", item)

		max_reset_bought = item_data["maxResetBuy"]
		purchase_cost = (cost_data := item_data["purchaseData"])["baseCost"] + (num_bought * cost_data["levelIncrease"])

		if num_bought >= max_reset_bought or bounty_points < purchase_cost:
			return Response(utils.compress({"message": "Bought max amount"}), status=400)

		mongo.db.bountyShop.update_one({"userId": uid}, {"$inc": {f"itemsBought.{item}": 1}}, upsert=True)

		mongo.db.userItems.update_one({"userId": uid}, {"$inc": {"bountyPoints": -purchase_cost}}, upsert=True)

		results = {
			BountyShopItem.PRESTIGE_POINTS: lambda u: add_prestige_points(u, item_data["maxStagePercent"]),
			BountyShopItem.GEMS: lambda u: add_items(u, gems=item_data["gems"]),
			BountyShopItem.WEAPON_POINTS: lambda u: add_items(u, weaponPoints=item_data["weaponPoints"]),
			BountyShopItem.WEAPON: lambda u: add_random_weapon(u)

		}.get(item)(uid)

		return Response(utils.compress(results), status=200)


def add_prestige_points(uid, max_stage_percent):
	items = mongo.db.userItems.find_one({"userId": uid}) or dict()
	stats = mongo.db.userStats.find_one({"userId": uid}) or dict()

	stage = stats.get("maxPrestigeStage", 0) * max_stage_percent

	points = max(100, formulas.stage_prestige_points(stage, items.get("loot", dict())))

	pp = int(items.get("prestigePoints", 0)) + points

	mongo.db.userItems.update_one({"userId": uid}, {"$set": {"prestigePoints": str(pp)}}, upsert=True)

	return {"prestigePointsReceived": str(points)}


def add_random_weapon(uid):

	weapon = int(random.choice(tuple(GameData.get("armoury").keys())))

	mongo.db.userItems.update_one({"userId": uid}, {"$inc": {f"weapons.{weapon}": 1}}, upsert=True)

	return {"weaponReceived": weapon}


def add_items(userid, **kwargs):

	mongo.db.userItems.update_one({"userId": userid}, {"$inc": kwargs}, upsert=True)

	return {f"{k}Received": v for k, v in kwargs.items()}
