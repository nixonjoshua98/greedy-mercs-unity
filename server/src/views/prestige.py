import math

from pymongo import ReturnDocument

from flask import Response, request, current_app as app

from flask.views import View

from src import utils, checks


class Prestige(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		data = utils.decompress(request.data)

		prestige_stage = data["prestigeStage"]

		if prestige_stage < 75:
			return Response(utils.compress({"message": ""}), status=400)

		prestige_points = self.calc_prestige_points(prestige_stage)

		# - Perform the prestige on the database
		user_items = app.mongo.db.userItems.find_one_and_update(
			{
				"userId": userid
			},
			{
				"$inc": {
					"prestigePoints": prestige_points
				}
			},
			upsert=True,
			return_document=ReturnDocument.AFTER
		)

		return Response(utils.compress({"prestigePoints": user_items["prestigePoints"]}), status=200)

	@staticmethod
	def calc_prestige_points(stage):
		return math.ceil(math.pow(math.ceil((stage - 70) / 10.0), 2))
