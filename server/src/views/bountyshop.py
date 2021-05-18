from flask import Response

from flask.views import View

from src import utils, checks, formulas

from src.classes.gamedata import GameData
from src.exts import mongo

from src.classes import chests
from src.enums import BountyShopItemID


class BountyShopRefresh(View):

	@checks.login_check
	def dispatch_request(self, *, uid):
		shop = mongo.db.dailyResetPurchases.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()

		if shop.get("lastReset") is None or shop["lastReset"] < GameData.last_daily_reset:
			shop = {"lastReset": GameData.last_daily_reset, "itemsBought": dict()}

			mongo.db.dailyResetPurchases.update_one({"userId": uid}, {"$set": shop}, upsert=True)

		return Response(utils.compress(shop), status=200)


class BountyShopItem:
	def __init__(self, key: str):
		self._data = GameData.get_item("bountyShopItems", key)

		self.daily_limit = self._data["dailyPurchaseLimit"]

		self.shop_id = key

	def purchase_cost(self, purchased: int) -> int:
		purchaseData = self._data["purchaseData"]

		return purchaseData["baseCost"] + (purchaseData["purchaseIncrease"] * purchased)

	def __getitem__(self, item):
		return self._data[item]


class BountyShop(View):

	@checks.login_check
	def dispatch_request(self, *, uid, data):

		item = BountyShopItem(str(data["itemId"]))

		# - Load data from database
		shop 	= mongo.db.dailyResetPurchases.find_one({"userId": uid}) or dict()
		items 	= mongo.db.inventories.find_one({"userId": uid}) or dict()

		# - Extract data from the database
		bounty_points 	= items.get("bountyPoints", 0)
		num_bought 		= shop.get("itemsBought", dict()).get(item.shop_id, 0)
		bp_cost 		= item.purchase_cost(num_bought)

		# - User has already reached the daily purchase limit
		if bp_cost > bounty_points:
			return Response(utils.compress({"message": "."}), status=400)

		mongo.db.inventories.update_one({"userId": uid}, {"$inc": {"bountyPoints": -bp_cost}}, upsert=True)
		mongo.db.dailyResetPurchases.update_one({"userId": uid}, {"$inc": {f"itemsBought.{item.shop_id}": 1}}, upsert=True)

		result = self.give_item(uid, item, items=items)

		return Response(utils.compress(result), status=200)

	def give_item(self, uid, itemdata: BountyShopItem, *, items) -> dict:
		shop_item: BountyShopItemID = BountyShopItemID(int(itemdata.shop_id))

		# - Armoury Chests
		if 300 <= shop_item.value < 400:
			return add_armoury_item(uid, itemdata=itemdata)

		return {
			BountyShopItemID.PRESTIGE_POINTS: 	lambda: add_prestige_points(uid, itemdata["maxStagePercent"], items=items),
			BountyShopItemID.GEMS: 				lambda: add_items(uid, gems=itemdata["gems"]),
			BountyShopItemID.ARMOURY_POINTS: 	lambda: add_items(uid, armouryPoints=itemdata["armouryPoints"]),
		}.get(shop_item)()


def add_prestige_points(uid, max_stage_percent, *, items):
	stats = mongo.db.userStats.find_one({"userId": uid}) or dict()
	stage = stats.get("maxPrestigeStage", 0) * max_stage_percent

	points = max(100, formulas.stage_prestige_points(stage, items.get("loot", dict())))

	pp = int(items.get("prestigePoints", 0)) + points

	mongo.db.inventories.update_one({"userId": uid}, {"$set": {"prestigePoints": str(pp)}}, upsert=True)

	return {"prestigePointsReceived": str(points)}


def add_armoury_item(uid, *, itemdata):
	item = chests.armoury_chest(mintier=itemdata["minTier"], maxtier=itemdata["maxTier"])

	mongo.db["userArmouryItems"].update_one({"userId": uid}, {"$inc": {f"items.{item}.owned": 1}}, upsert=True)

	return {"itemReceived": item}


def add_items(uid, **kwargs):
	mongo.db.inventories.update_one({"userId": uid}, {"$inc": kwargs}, upsert=True)

	return {f"{k}Received": v for k, v in kwargs.items()}
