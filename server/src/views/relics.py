import math
import random

from pymongo import ReturnDocument

from flask import Response, request, current_app as app
from flask.views import View

from src import utils, checks

NUM_RELICS = 7


class BuyRelic(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		# - Load user data from the database
		items = app.mongo.db.userItems.find_one({"userId": userid}) or dict()

		prestigePointsString = items.get("prestigePoints", 0)

		relic, cost = self.get_next_relic(items.get("relics", []))

		# - No relic available
		if relic is None or cost is None:
			return Response(utils.compress({"message": ""}), status=400)

		# - User cannot afford the next relic
		if int(prestigePointsString) < cost:
			return Response(utils.compress({"message": ""}), status=400)

		remainPrestigePoints = int(prestigePointsString) - cost

		updatedUserItems = app.mongo.db.userItems.find_one_and_update(
			{
				"userId": userid
			},

			{
				"$set": {"prestigePoints": str(remainPrestigePoints)},
				"$push": {"relics": {"relicId": relic, "level": 1}}
			},

			return_document=ReturnDocument.AFTER,
			upsert=True
		)

		return Response(
			utils.compress({
				"relicBought": relic,
				"prestigePoints": str(updatedUserItems["prestigePoints"])
			}),
			status=200
		)

	def get_next_relic(self, relics):

		if len(relics) == NUM_RELICS:
			return None, None

		owned = [relic["relicId"] for relic in relics]

		all_relics = [i for i in range(NUM_RELICS)]

		available = list(set(all_relics) - set(owned))

		return random.choice(available), math.floor(math.pow(1.5, len(relics)))


class UpgradeRelic(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		data = utils.decompress(request.data)

		# - Load user data from the database
		items = app.mongo.db.userItems.find_one({"userId": userid}) or dict()

		ownedRelics = items.get("relics", [])

		prestigePointsString = items.get("prestigePoints", 0)  # prestigePoints are stored as a string

		relicEntry = utils.get(ownedRelics, relicId=data["relicId"])

		# - User is trying to upgrade an invalid relic or one they do not currently own
		if relicEntry is None:
			return Response(utils.compress({"message": "You do not own this relic"}), status=400)

		cost = self.get_relic_cost(self.get_relic_data(data["relicId"]), relicEntry["level"], data["buyLevels"])

		# - User cannot afford to upgrade
		if cost > int(prestigePointsString):
			return Response(utils.compress({"message": "You cannot afford to upgrade this relic"}), status=400)

		remainPrestigePoints = int(prestigePointsString) - cost

		updatedUserItems = app.mongo.db.userItems.find_one_and_update(
			{
				"userId": userid,
				"relics.relicId": data["relicId"]
			},

			{
				"$set": {"prestigePoints": str(remainPrestigePoints)},
				"$inc": {"relics.$.level": data["buyLevels"]}
			},

			return_document=ReturnDocument.AFTER,
			upsert=True
		)

		updatedRelicEntry = utils.get(updatedUserItems.get("relics", []), relicId=data["relicId"])

		return Response(
			utils.compress({
				"relicLevel": updatedRelicEntry["level"],
				"prestigePoints": str(updatedUserItems.get("prestigePoints", 0))
			}),
			status=200
		)

	@staticmethod
	def get_relic_data(relic):

		return app.data["static"]["relics"].get(str(relic))

	@staticmethod
	def get_relic_cost(relic_data, start, levels):

		base_cost = relic_data["baseCost"]
		cost_power = relic_data["costPower"]

		val = base_cost * math.pow(cost_power, start - 1) * (1 - math.pow(cost_power, levels)) / (1 - cost_power)

		return math.floor(val)
