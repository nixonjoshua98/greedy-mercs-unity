import random

from flask import Response, request, current_app as app
from flask.views import View

from src import utils, checks, formulas


class BuyLoot(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		# - Load user data from the database
		items = app.mongo.db.userItems.find_one({"userId": userid}) or dict()

		prestige_points = int(items.get("prestigePoints", 0))

		loot_items = items.get("loot", dict())

		# - No item available
		if len(loot_items) == len(app.objects["loot"]):
			return Response(utils.compress({"message": ""}), status=400)

		# - User cannot afford the next item
		if prestige_points < (cost := formulas.next_prestige_item_cost(len(loot_items))):
			return Response(utils.compress({"message": ""}), status=400)

		new_item_id = self.get_random_item(loot_items)

		remainPrestigePoints = prestige_points - cost

		app.mongo.db.userItems.update_one(
			{"userId": userid},
			{"$set": {"prestigePoints": str(remainPrestigePoints), f"loot.{new_item_id}": 1}},
			upsert=True
		)

		return_data = {"newLootId": new_item_id, "prestigePoints": str(remainPrestigePoints)}

		return Response(utils.compress(return_data), status=200)

	def get_random_item(self, items: dict):

		owned = [int(k) for k in items.keys()]

		all_items = [int(k) for k, v in app.objects["loot"].items()]

		available = list(set(all_items) - set(owned))

		return random.choice(available)


class UpgradeLoot(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		data = utils.decompress(request.data)

		levels_buying = data["buyLevels"]

		# - Load user data from the database
		items = app.mongo.db.userItems.find_one({"userId": userid}) or dict()

		loot_items = {int(k): v for k, v in items.get("loot", dict()).items()}

		prestige_points = int(items.get("prestigePoints", 0))  # prestigePoints are stored as a string

		static_item = app.objects["loot"][data["itemId"]]

		# - User is trying to upgrade an invalid item or one they do not currently own
		if (item_level := loot_items.get(data["itemId"])) is None:
			return Response(utils.compress({"message": "You do not own this item"}), status=400)

		elif (item_level + levels_buying) > static_item.max_level:
			return Response(utils.compress({"message": "Buying will exceed the item max level"}), status=400)

		cost = static_item.levelup(item_level, data["buyLevels"])

		# - User cannot afford to upgrade
		if cost > prestige_points:
			return Response(utils.compress({"message": "You cannot afford to upgrade this item"}), status=400)

		remain_prestige_points = prestige_points - cost

		app.mongo.db.userItems.update_one(
			{"userId": userid},

			{
				"$set": {"prestigePoints": str(remain_prestige_points)},
				"$inc": {f"loot.{data['itemId']}": data["buyLevels"]}
			},

			upsert=True
		)

		return Response(utils.compress({
				"itemLevel": 		item_level + data["buyLevels"],
				"prestigePoints": 	str(remain_prestige_points)
			}),

			status=200
		)
