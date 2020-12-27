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

		items = app.mongo.db.userItems.find_one({"userId": userid}) or dict()

		relic, cost = self.get_next_relic(items.get("relics", []))

		# - No relic available
		if relic is None or cost is None:
			return Response(utils.compress({"message": ""}), status=400)

		# - User cannot afford the next relic
		if items.get("prestigePoints", 0) < cost:
			return Response(utils.compress({"message": ""}), status=400)

		items = app.mongo.db.userItems.find_one_and_update(
			{
				"userId": userid
			},

			{
				"$inc": {"prestigePoints": -cost},
				"$push": {"relics": {"relicId": relic, "level": 1}}
			},

			return_document=ReturnDocument.AFTER,
			upsert=True
		)

		return Response(utils.compress({"relicBought": relic, "prestigePoints": items["prestigePoints"]}), status=200)

	def get_next_relic(self, relics):

		if len(relics) == NUM_RELICS:
			return None, None

		owned = [relic["relicId"] for relic in relics]

		all_relics = [i for i in range(NUM_RELICS)]

		available = list(set(all_relics) - set(owned))

		return random.choice(available), int(math.pow(2, len(relics)))


class UpgradeRelic(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		data = utils.decompress(request.data)

		items = app.mongo.db.userItems.find_one({"userId": userid}) or dict()

		owned_relics = items.get("relics", dict())

		relic_index, relic_entry = utils.get(owned_relics, relicId=data["relicId"])

		if relic_entry is None:
			return Response(utils.compress({"message": "You do not own this relic"}), status=400)

		# - Invalid relic ID
		elif (relic_data := self.get_relic_data(data["relicId"])) is None:
			return Response(utils.compress({"message": "Attempting to upgrade an invalid relic"}), status=400)

		cost = self.get_relic_cost(relic_data, relic_entry["level"], data["buyLevels"])

		# - User cannot afford to upgrade
		if cost > items.get("prestigePoints", 0):
			return Response(utils.compress({"message": "You cannot afford to upgrade this relic"}), status=400)

		items = app.mongo.db.userItems.find_one_and_update(
			{
				"userId": userid,
				"relics.relicId": data["relicId"]
			},

			{
				"$inc": {"prestigePoints": -cost, "relics.$.level": data["buyLevels"]}
			},

			return_document=ReturnDocument.AFTER,
			upsert=True
		)

		owned_relics = items.get("relics", dict())

		_, relic_entry = utils.get(owned_relics, relicId=data["relicId"])

		return Response(
			utils.compress({
				"relicLevel": relic_entry["level"],
				"prestigePoints": items.get("prestigePoints", 0)
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

		return math.ceil(val)


