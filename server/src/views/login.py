from flask import Response, request, current_app as app

from flask.views import View

from src import utils


class Login(View):

	def dispatch_request(self):

		data = utils.decompress(request.data)

		# - New player (first login)
		if (row := app.mongo.db.userLogins.find_one({"deviceId": data["deviceId"]})) is None:
			result = app.mongo.db.userLogins.insert_one({"deviceId": data["deviceId"]})

			user_id = result.inserted_id

		else:
			user_id = row["_id"]

		items = app.mongo.db.userItems.find_one({"userId": user_id}) or dict()

		return Response(
			utils.compress(
				{
					"prestigePoints": items.get("prestigePoints", 0),
					"relics": items.get("relics", [])
				},
			),

			status=200)

