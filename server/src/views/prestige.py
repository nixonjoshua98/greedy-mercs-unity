from pymongo import ReturnDocument

from flask import Response, request

from flask.views import View

from src import utils


class Prestige(View):

	def __init__(self, mongo):

		self.mongo = mongo

	def dispatch_request(self):

		data = utils.decompress(request.data)

		prestige_stage = data["prestigeStage"]

		if (row := self.mongo.db.userLogins.find_one({"deviceId": data["deviceId"]})) is None:
			return Response(utils.compress({"message": ""}), status=400)

		elif prestige_stage < 75:
			return Response(utils.compress({"message": ""}), status=400)

		prestige_points = utils.formulas.calc_prestige_points(prestige_stage)

		# - Perform the prestige on the database
		user_items = self.mongo.db.userItems.find_one_and_update(
			{
				"userId": row["_id"]
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