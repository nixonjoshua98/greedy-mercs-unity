import math

from pymongo import ReturnDocument

from flask import Response, request, current_app as app

from flask.views import View

from src import utils, checks

MIN_PRESTIGE_STAGE = 80


class Prestige(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		data = utils.decompress(request.data)

		prestige_stage = data["prestigeStage"]

		if prestige_stage < MIN_PRESTIGE_STAGE:
			return Response(utils.compress({"message": ""}), status=400)

		prestigePointsEarned = self.calc_prestige_points(prestige_stage)

		userItems = app.mongo.db.userItems.find_one({"userId": userid}) or dict()

		# - Perform the prestige on the database
		userItems = app.mongo.db.userItems.find_one_and_update(
			{
				"userId": userid
			},
			{
				"$set": {
					"prestigePoints": str(int(userItems.get("prestigePoints", 0)) + prestigePointsEarned)
				}
			},
			upsert=True,
			return_document=ReturnDocument.AFTER
		)

		return Response(utils.compress({"prestigePoints": userItems["prestigePoints"]}), status=200)

	@staticmethod
	def calc_prestige_points(stage):
		return math.ceil(math.pow(math.ceil((stage - MIN_PRESTIGE_STAGE) / 10.0), 2.0))
