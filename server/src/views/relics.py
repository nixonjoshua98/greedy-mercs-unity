import random

from flask import Response, request, current_app as app
from flask.views import View

from src import utils, checks, formulas


class BuyRelic(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		# - Load user data from the database
		items = app.mongo.db.userItems.find_one({"userId": userid}) or dict()

		prestige_points = int(items.get("prestigePoints", 0))

		relics = items.get("relics", dict())

		# - No relic available
		if len(relics) == len(app.objects["relics"]):
			return Response(utils.compress({"message": ""}), status=400)

		# - User cannot afford the next relic
		if prestige_points < (cost := formulas.next_relic_cost(len(relics))):
			return Response(utils.compress({"message": ""}), status=400)

		new_relic_id = self.get_next_relic(relics)

		remainPrestigePoints = prestige_points - cost

		app.mongo.db.userItems.update_one(
			{"userId": userid},
			{"$set": {"prestigePoints": str(remainPrestigePoints), f"relics.{new_relic_id}": 1}},
			upsert=True
		)

		return_data = {"relicBought": new_relic_id, "prestigePoints": str(remainPrestigePoints)}

		return Response(utils.compress(return_data), status=200)

	def get_next_relic(self, relics: dict):

		owned = [int(k) for k in relics.keys()]

		all_relics = [int(k) for k, v in app.objects["relics"].items()]

		available = list(set(all_relics) - set(owned))

		return random.choice(available)


class UpgradeRelic(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		data = utils.decompress(request.data)

		levels_buying = data["buyLevels"]

		# - Load user data from the database
		items = app.mongo.db.userItems.find_one({"userId": userid}) or dict()

		user_relics = {int(k): v for k, v in items.get("relics", dict()).items()}

		prestige_points = int(items.get("prestigePoints", 0))  # prestigePoints are stored as a string

		static_relic = app.objects["relics"][data["relicId"]]

		# - User is trying to upgrade an invalid relic or one they do not currently own
		if (relic_level := user_relics.get(data["relicId"])) is None:
			return Response(utils.compress({"message": "You do not own this relic"}), status=400)

		elif (relic_level + levels_buying) > static_relic.max_level:
			return Response(utils.compress({"message": "Buying will exceed the relic max level"}), status=400)

		cost = static_relic.levelup(relic_level, data["buyLevels"])

		# - User cannot afford to upgrade
		if cost > prestige_points:
			return Response(utils.compress({"message": "You cannot afford to upgrade this relic"}), status=400)

		remain_prestige_points = prestige_points - cost

		app.mongo.db.userItems.update_one(
			{"userId": userid},

			{
				"$set": {"prestigePoints": str(remain_prestige_points)},
				"$inc": {f"relics.{data['relicId']}": data["buyLevels"]}
			},

			upsert=True
		)

		return_data = {"relicLevel": relic_level + data["buyLevels"], "prestigePoints": str(remain_prestige_points)}

		return Response(utils.compress(return_data), status=200)
