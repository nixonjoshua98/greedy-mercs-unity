from flask import Response, request, current_app as app

from flask.views import View

from src import utils, checks, formulas


class Prestige(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		data = utils.decompress(request.data)

		userItems = app.mongo.db.userItems.find_one({"userId": userid}) or dict()

		prestige_points, user_relics = userItems.get("prestigePoints", 0), userItems.get("relics", dict())

		points_earned = formulas.stage_prestige_points(data["prestigeStage"], app.objects["relics"], user_relics)

		new_prestige_points = int(userItems.get("prestigePoints", 0)) + points_earned

		# - Perform the prestige on the database
		app.mongo.db.userItems.update_one(
			{"userId": userid},
			{"$set": {"prestigePoints": str(new_prestige_points)}},
			upsert=True,
		)

		return Response(utils.compress({"prestigePoints": str(new_prestige_points)}), status=200)
