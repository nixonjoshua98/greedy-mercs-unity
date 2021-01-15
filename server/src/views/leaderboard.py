from flask import Response, current_app as app

from flask.views import View

from src import utils


class PlayerLeaderboard(View):

	def dispatch_request(self):

		pipeline = [
			{
				"$group": {
					"_id": "$userId",

					"doc": {"$first": "$$ROOT"}
				}
			},
			{
				"$sort": {"doc.maxPrestigeStage": -1}
			},
			{
				"$limit": 10
			},
			{
				"$project": {
					"_id": 0,
					"username": {"$ifNull": ["$doc.username", "Rogue Mercenary"]},
					"maxPrestigeStage": "$doc.maxPrestigeStage",
				}
			}
		]

		results = list(app.mongo.db.userStats.aggregate(pipeline))

		return Response(utils.compress({"players": results}), status=200)
