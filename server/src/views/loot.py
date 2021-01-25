import random

from flask import Response, request, current_app as app
from flask.views import View

from src import utils, checks, formulas

from src.classes import StaticGameData


class BuyLoot(View):

	@checks.login_check
	def dispatch_request(self, uid):

		# - Load user data from the database
		items = app.mongo.db.userItems.find_one({"userId": uid}) or dict()

		# - Load static data
		loot_static_data = StaticGameData.get("loot")

		prestige_points = int(items.get("prestigePoints", 0))

		user_loot = items.get("loot", dict())

		# - No item available
		if len(user_loot) == len(loot_static_data):
			return Response(utils.compress({"message": ""}), status=400)

		# - User cannot afford the next item
		if prestige_points < (cost := formulas.next_loot_item_cost(len(user_loot))):
			return Response(utils.compress({"message": ""}), status=400)

		new_item_id = self.get_random_item(user_loot)

		remainPrestigePoints = prestige_points - cost

		app.mongo.db.userItems.update_one(
			{"userId": uid},
			{"$set": {"prestigePoints": str(remainPrestigePoints), f"loot.{new_item_id}": 1}},
			upsert=True
		)

		return_data = {"newLootId": new_item_id, "prestigePoints": str(remainPrestigePoints)}

		return Response(utils.compress(return_data), status=200)

	def get_random_item(self, items: dict):

		loot_items = StaticGameData.get("loot")

		available = list(set(list(loot_items.keys())) - set(list(items.keys())))

		return random.choice(available)


class UpgradeLoot(View):

	@checks.login_check
	def dispatch_request(self, *, uid):

		data = utils.decompress(request.data)

		item_buying		= str(data["itemId"])  # IDs are stored as strings (sicne JSON) so convert to follow suit
		levels_buying 	= data["buyLevels"]

		# - Load user data from the database
		items = app.mongo.db.userItems.find_one({"userId": uid}) or dict()

		loot_items = {int(k): v for k, v in items.get("loot", dict()).items()}

		prestige_points = int(items.get("prestigePoints", 0))  # prestigePoints are stored as a string

		staticdata = StaticGameData.get_item("loot", item_buying)

		# - User is trying to upgrade an invalid item or one they do not currently own
		if (item_level := loot_items.get(data["itemId"])) is None:
			return Response(utils.compress({"message": "You do not own this item"}), status=400)

		elif (item_level + levels_buying) > staticdata.get("maxLevel", float("inf")):
			return Response(utils.compress({"message": "."}), status=400)

		cost = formulas.loot_levelup_cost(staticdata, item_level, levels_buying)

		# - User cannot afford to upgrade
		if cost > prestige_points:
			return Response(utils.compress({"message": "."}), status=400)

		remain_prestige_points = prestige_points - cost

		app.mongo.db.userItems.update_one(
			{"userId": uid},

			{
				"$set": {"prestigePoints": str(remain_prestige_points)},
				"$inc": {f"loot.{data['itemId']}": data["buyLevels"]}
			},

			upsert=True
		)

		return "200", 200
