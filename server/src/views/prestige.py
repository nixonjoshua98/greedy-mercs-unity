from flask import Response, request, current_app as app

from flask.views import View

from src import utils, checks, formulas


class Prestige(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		data = utils.decompress(request.data)

		stage = data["prestigeStage"]

		userItems = app.mongo.db.userItems.find_one({"userId": userid}) or dict()

		prestige_points, user_relics = userItems.get("prestigePoints", 0), userItems.get("relics", dict())

		points_earned = formulas.stage_prestige_points(stage, app.objects["relics"], user_relics)

		pp = int(userItems.get("prestigePoints", 0)) + points_earned

		utils.dbops.update_max_prestige_stage(app.mongo, userid, stage)

		app.mongo.db.userItems.update_one({"userId": userid}, {"$set": {"prestigePoints": str(pp)}}, upsert=True)

		return Response(utils.compress(utils.dbops.get_player_data(app.mongo, userid)), status=200)
