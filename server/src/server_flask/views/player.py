from flask import Response, request

from flask.views import View

from src.server_flask import utils, checks
from src.exts import mongo
from src.server_flask.utils import dbops


class PlayerLogin(View):

	def dispatch_request(self):
		data = utils.decompress(request.data)

		# - New login detected
		if (row := mongo.db.userLogins.find_one({"deviceId": data["deviceId"]})) is None:
			result = mongo.db.userLogins.insert_one({"deviceId": data["deviceId"]})

			uid = result.inserted_id

		else:
			uid = row["_id"]

		return Response(utils.compress(dbops.get_player_data(uid)), status=200)


class ChangeUsername(View):

	@checks.login_check
	def dispatch_request(self, uid):

		data = utils.decompress(request.data)

		mongo.db.userInfo.update_one({"userId": uid}, {"$set": {"username": data["newUsername"]}}, upsert=True)

		return "OK", 200
