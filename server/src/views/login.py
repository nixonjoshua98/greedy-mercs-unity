from flask import Response, request, current_app as app

from flask.views import View

from src import utils


class Login(View):

	def dispatch_request(self):

		data = utils.decompress(request.data)

		if (row := app.mongo.db.userLogins.find_one({"deviceId": data["deviceId"]})) is None:
			result = app.mongo.db.userLogins.insert_one({"deviceId": data["deviceId"]})

			user_id = result.inserted_id

		else:
			user_id = row["_id"]

		items = app.mongo.db.userItems.find_one({"userId": user_id}) or dict()

		return Response(
			utils.compress(
				{
					"relics": items.get("relics", []),
					"bountyPoints": items.get("bountyPoints", 0),
					"prestigePoints": str(items.get("prestigePoints", 0))
				},
			),

			status=200)