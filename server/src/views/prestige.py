from flask import Response, request, current_app as app

from flask.views import View

from src import formulas, utils, checks


class Prestige(View):

	@checks.login_check
	def dispatch_request(self, *, uid):

		data = utils.decompress(request.data)

		stage = data["prestigeStage"]

		# - Load data from the database
		stats = app.mongo.db.userStats.find_one({"userId": uid})
		items = app.mongo.db.userItems.find_one({"userId": uid}) or dict()

		utils.dbops.add_bounty_prestige_levels(uid, stage)

		# - Add prestige points earned
		pp = int(items.get("prestigePoints", 0)) + formulas.stage_prestige_points(stage, items.get("loot", dict()))
		app.mongo.db.userItems.update_one({"userId": uid}, {"$set": {"prestigePoints": str(pp)}}, upsert=True)

		# - Update max prestige stage
		if stats is None or stats.get("maxPrestigeStage", 0) < stage:
			app.mongo.db.userStats.update_one({"userId": uid}, {"$set": {"maxPrestigeStage": stage}}, upsert=True)

		return Response(utils.compress(utils.dbops.get_player_data(uid)), status=200)
