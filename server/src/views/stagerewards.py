import json

from flask import jsonify, request

from flask.views import View


from src import utils
from src.databasequeries import DatabaseQueries


class StageRewards(View):

	def __init__(self, mongo):
		self.mongo = mongo

	def dispatch_request(self):

		data = request.get_json()

		stageReached = data["stageReached"]

		if (user := DatabaseQueries.find_user(self.mongo, device=data.get("deviceId"))) is None:
			return jsonify({"message": " "}), 400

		stats = self.mongo.db.userStats.find_one({"userId": user["userId"]}) or dict()

		if data["stageReached"] <= stats.get("lastStageRewarded", 0):
			return jsonify({"message": " "}), 400

		rewards = {int(k): v for k, v in json.loads(utils.File.read_data_file("herounlocks.json")).items()}

		# No reward available
		if (reward := rewards.get(stageReached)) is None:
			return jsonify({"message": " "}), 400

		# Update the database to reflect the stage
		self.mongo.db.userStats.update_one(
			{
				"userId": user["userId"]
			},
			{
				"$set": {"lastStageRewarded": stageReached}
			},
			upsert=True
		)

		self.mongo.db.heroes.insert_one({"userId": user["userId"], "heroId": reward["heroId"]})

		return jsonify({"reward": reward, "lastStageRewarded": stageReached}), 200


