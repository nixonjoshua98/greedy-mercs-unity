import math
import random

from flask import Response, request
from flask.views import View

from src import utils

RELIC_COSTS = [
	1
]


class BuyRelic(View):

	def __init__(self, mongo):

		self.mongo = mongo

	def dispatch_request(self):

		data = utils.decompress(request.data)

		# - Login
		if (row := self.mongo.db.userLogins.find_one({"deviceId": data["deviceId"]})) is None:
			return Response(utils.compress({"message": ""}), status=400)

		items = self.mongo.db.userItems.find_one({"userId": row["_id"]}) or dict()

		relic, cost = self.get_next_relic(items.get("relics", []))

		# - No relic available
		if relic is None or cost is None:
			return Response(utils.compress({"message": ""}), status=400)

		# - User cannot afford the next relic
		if items.get("prestigePoints", 0) < cost:
			return Response(utils.compress({"message": ""}), status=400)

		self.mongo.db.userItems.update_one(
			{
				"userId": row["_id"]
			},

			{
				"$inc": {"prestigePoints": -cost},
				"$push": {"relics": {"relicId": relic, "level": 1}}
			}
		)

		return Response(utils.compress({"boughtRelic": relic}), status=200)

	def get_next_relic(self, relics):

		if len(relics) == len(RELIC_COSTS):
			return None, None

		owned = [relic["relicId"] for relic in relics]

		all_relics = [i for i in range(len(RELIC_COSTS))]

		available = list(set(all_relics) - set(owned))

		return random.choice(available), int(math.pow(2, len(relics)))


class UpgradeRelic(View):

	def dispatch_request(self):
		return "OK"
