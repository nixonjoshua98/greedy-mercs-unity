from flask import Response, request

from flask.views import View

from src import formulas, utils, checks
from src.utils import dbops
from src.exts import mongo


class Prestige(View):

	@checks.login_check
	def dispatch_request(self, *, uid):

		data = utils.decompress(request.data)

		stage = data["prestigeStage"]

		# - Load data from the database
		stats = mongo.db.userStats.find_one({"userId": uid})
		items = mongo.db.userItems.find_one({"userId": uid}) or dict()

		# - Add Bounty levels gained
		levels = (mongo.db.userBounties.find_one({"userId": uid}) or dict()).get("bountyLevels", dict())

		levels_earned = formulas.stage_bounty_levels(stage, levels)

		if levels_earned:
			query = {f"bountyLevels.{key}": level for key, level in levels_earned.items()}

			mongo.db.userBounties.update_one({"userId": uid}, {"$inc": query})

		# - Add prestige points earned
		pp = int(items.get("prestigePoints", 0)) + formulas.stage_prestige_points(stage, items.get("loot", dict()))

		mongo.db.userItems.update_one({"userId": uid}, {"$set": {"prestigePoints": str(pp)}}, upsert=True)

		# - Update max prestige stage
		if stats is None or stats.get("maxPrestigeStage", 0) < stage:
			mongo.db.userStats.update_one({"userId": uid}, {"$set": {"maxPrestigeStage": stage}}, upsert=True)

		return Response(utils.compress(dbops.get_player_data(uid)), status=200)
