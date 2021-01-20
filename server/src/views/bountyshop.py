import math

from flask import request, Response, current_app as app

from src import utils, checks, formulas

from src.enums import BountyShopItem


class BountyShop:

	@classmethod
	def add_routes(cls, app):
		app.add_url_rule("/api/bountyshop/refresh", "bountyshop.refresh", view_func=cls.refresh_shop, methods=["PUT"])
		app.add_url_rule("/api/bountyshop/buy", "bountyshop.buyitem", view_func=cls.buy_item, methods=["PUT"])

	@classmethod
	@checks.login_check
	def refresh_shop(cls, *, userid):
		shop = utils.dbops.get_bounty_shop_and_update(userid)

		return Response(utils.compress(shop), status=200)

	@classmethod
	@checks.login_check
	def buy_item(cls, *, userid):

		data = utils.decompress(request.data)

		shop = utils.dbops.get_bounty_shop_and_update(userid)

		items = app.mongo.db.userItems.find_one({"userId": userid}) or dict()

		item = data["itemId"]

		bounty_points 	= items.get("bountyPoints", 0)
		num_bought 		= shop["itemsBought"][str(item)]

		max_reset_bought 	= app.staticdata["bountyShopItems"][str(item)]["maxResetBuy"]
		purchase_cost 		= app.staticdata["bountyShopItems"][str(item)]["purchaseCost"]

		if num_bought >= max_reset_bought or bounty_points < purchase_cost:
			return Response(utils.compress({"message": "Bought max amount"}), status=400)

		app.mongo.db.userBountyShop.update_one({"userId": userid}, {"$inc": {f"itemsBought.{item}": 1}})

		app.mongo.db.userItems.update_one({"userId": userid}, {"$inc": {"bountyPoints": -purchase_cost}})

		results = {
			BountyShopItem.PRESTIGE_90: lambda u: add_prestige_points(u, max_stage_percent=0.9)

		}.get(item)(userid)

		return Response(utils.compress(results), status=200)


def add_prestige_points(userid, *, max_stage_percent) -> int:
	items = app.mongo.db.userItems.find_one({"userId": userid}) or dict()
	stats = app.mongo.db.userStats.find_one({"userId": userid}) or dict()

	stage = math.ceil(max(100, stats.get("maxPrestigeStage", 0)) * max_stage_percent)

	points = formulas.calc_stage_prestige_points(stage, items.get("loot", dict()))

	pp = int(items.get("prestigePoints", 0)) + points

	app.mongo.db.userItems.update_one({"userId": userid}, {"$set": {"prestigePoints": str(pp)}}, upsert=True)

	return {"receivedPrestigePoints": str(points)}