from flask import Response

from flask.views import View

from src import utils
from src.exts import mongo


class PlayerLeaderboard(View):

	def dispatch_request(self):

		results = mongo.db.userStats.find(
			{
				"maxPrestigeStage": {"$exists": True}
			},
			{
				"_id": 0,
				"userId": 0
			}

		).sort("maxPrestigeStage", -1).limit(20)

		return Response(utils.compress({"players": list(results)}), status=200)