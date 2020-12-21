from flask import jsonify, request
from flask.views import View

from src.data.enums import HeroID

from src.databasequeries import DatabaseQueries


class Login(View):

	def __init__(self, mongo):
		self.mongo = mongo

	def dispatch_request(self):

		data = request.get_json()

		if (device := data.get("deviceId")) is None:
			return jsonify({"message": " "}), 400

		user = DatabaseQueries.find_user(self.mongo, device=device)

		if user is None:
			result = self.mongo.db.users.update_one({"deviceId": device}, {"$set": {"deviceId": device}}, upsert=True)

			user_id = result.upserted_id

			self.mongo.db.heroes.insert_many(
				[
					{"userId": user_id, "heroId": HeroID.WRAITH_LIGHTNING},
				]
			)

			stats = self.mongo.db.userStats.find_one({"userId": user_id}) or dict()

		else:
			stats = self.mongo.db.userStats.find_one({"userId": (user_id := user["userId"])}) or dict()

		data = {
			"heroes": DatabaseQueries.get_heroes(self.mongo, user_id),

			"lastStageRewarded": stats.get("lastStageRewarded", 0)
		}

		return jsonify(data)
