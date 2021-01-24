import random

from pymongo import ReturnDocument

from flask import request, Response, current_app as app

from flask.views import View

from src import utils, checks, formulas

from src.enums import BountyShopItem


class BountyShopRefresh(View):

	@checks.login_check
	def dispatch_request(self, uid):
		shop = app.mongo.db.bountyShop.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()

		last_reset = shop.get("lastPurchaseReset")

		if last_reset is None or app.last_daily_reset > last_reset:
			shop = app.mongo.db.bountyShop.find_one_and_update(
				{"userId": uid},
				{"$set": {"lastPurchaseReset": app.last_daily_reset}, "$unset": {"itemsBought": None}},

				projection={'_id': False, 'userId': False},
				upsert=True,
				return_document=ReturnDocument.AFTER
			)

			return Response(utils.compress(shop), status=200)

		return "400", 400


class BountyShop(View):

	@checks.login_check
	def dispatch_request(self, uid):

		data = utils.decompress(request.data)

		# - Load data from database
		shop 	= app.mongo.db.bountyShop.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()
		items 	= app.mongo.db.userItems.find_one({"userId": uid}) or dict()

		item = data["itemId"]

		bounty_points 	= items.get("bountyPoints", 0)
		num_bought 		= shop.get("itemsBought", dict()).get(str(item), 0)

		item_data = app.staticdata["bountyShopItems"][str(item)]

		max_reset_bought = item_data["maxResetBuy"]
		purchase_cost = (cost_data := item_data["purchaseData"])["baseCost"] + (num_bought * cost_data["levelIncrease"])

		if num_bought >= max_reset_bought or bounty_points < purchase_cost:
			return Response(utils.compress({"message": "Bought max amount"}), status=400)

		app.mongo.db.bountyShop.update_one({"userId": uid}, {"$inc": {f"itemsBought.{item}": 1}})

		app.mongo.db.userItems.update_one({"userId": uid}, {"$inc": {"bountyPoints": -purchase_cost}})

		results = {
			BountyShopItem.PRESTIGE_POINTS: lambda u: add_prestige_points(u, item_data["maxStagePercent"]),
			BountyShopItem.GEMS: lambda u: add_items(u, gems=item_data["gems"]),
			BountyShopItem.WEAPON_POINTS: lambda u: add_items(u, weaponPoints=item_data["weaponPoints"]),
			BountyShopItem.WEAPON: lambda u: add_random_weapon(u)

		}.get(item)(uid)

		return Response(utils.compress(results), status=200)


def add_prestige_points(uid, max_stage_percent):
	items = app.mongo.db.userItems.find_one({"userId": uid}) or dict()
	stats = app.mongo.db.userStats.find_one({"userId": uid}) or dict()

	stage = stats.get("maxPrestigeStage", 0) * max_stage_percent

	points = max(100, formulas.stage_prestige_points(stage, items.get("loot", dict())))

	pp = int(items.get("prestigePoints", 0)) + points

	app.mongo.db.userItems.update_one({"userId": uid}, {"$set": {"prestigePoints": str(pp)}}, upsert=True)

	return {"prestigePointsReceived": str(points)}


def add_random_weapon(uid):

	weapon = int(random.choice(tuple(app.staticdata["weapons"].keys())))

	app.mongo.db.userItems.update_one({"userId": uid}, {"$inc": {f"weapons.{weapon}": 1}}, upsert=True)

	return {"weaponReceived": weapon}


def add_items(userid, **kwargs):

	app.mongo.db.userItems.update_one({"userId": userid}, {"$inc": kwargs}, upsert=True)

	return {f"{k}Received": v for k, v in kwargs.items()}
