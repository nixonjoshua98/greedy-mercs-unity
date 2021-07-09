from flask import request

from flask.views import View

from src import dbutils
from src.common import mongo, checks


class PlayerLogin(View):

	def dispatch_request(self):

		data = request.get_json()

		# - New login detected
		if (row := mongo.db.userLogins.find_one({"deviceId": data["deviceId"]})) is None:
			result = mongo.db.userLogins.insert_one({"deviceId": data["deviceId"]})

			uid = result.inserted_id

		else:
			uid = row["_id"]

		return dbutils.get_player_data(uid)


class ChangeUsername(View):

	@checks.login_check
	def dispatch_request(self, uid, data):

		mongo.db.userInfo.update_one({"userId": uid}, {"$set": {"username": data["newUsername"]}}, upsert=True)

		return "OK", 200
