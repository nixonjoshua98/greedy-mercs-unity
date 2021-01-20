from flask import Response, request, current_app as app

from flask.views import View

from src import utils


class Login(View):

	def dispatch_request(self):
		data = utils.decompress(request.data)

		# - New login detected
		if (row := app.mongo.db.userLogins.find_one({"deviceId": data["deviceId"]})) is None:
			result = app.mongo.db.userLogins.insert_one({"deviceId": data["deviceId"]})

			uid = result.inserted_id

		else:
			uid = row["_id"]

		return Response(utils.compress(utils.dbops.get_player_data(uid)), status=200)