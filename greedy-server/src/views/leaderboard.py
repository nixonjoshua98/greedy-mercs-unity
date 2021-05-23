from flask import Response

from flask.views import View

from src import utils
from src.common import mongo


class PlayerLeaderboard(View):

	def dispatch_request(self):

		results = mongo.db.userStats.aggregate(
			[
				{"$match": {"maxPrestigeStage": {"$exists": True}}},
				{
					"$lookup": {
						"from": "userInfo",
						"localField": "userId",
						"foreignField": "userId",
						"as": "userInfoLookup"
					}
				},
				{"$unwind": {"path": "$userInfoLookup", "preserveNullAndEmptyArrays": True}},
				{"$project": {"_id": 0, "username": "$userInfoLookup.username", "maxPrestigeStage": 1}},
				{"$sort": {"maxPrestigeStage": -1}},
				{"$limit": 25}
			]
		)

		return Response(utils.compress({"players": list(results)}), status=200)