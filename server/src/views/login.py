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
		bounties = app.mongo.db.userBounties.find({"userId": user_id}, {"_id": 0, "userId": 0})

		bounties = list(bounties)  # - Convert from a cursor to a list

		# - Convert date to timestamp so we can jsonify it
		for i, _ in enumerate(bounties):
			bounties[i]["startTime"] = bounties[i]["startTime"].timestamp()

		print(user_id)

		return Response(
			utils.compress(
				{
					"relics": items.get("relics", []),
					"bounties": bounties,
					"bountyPoints": items.get("bountyPoints", 0),
					"prestigePoints": str(items.get("prestigePoints", 0))
				},
			),

			status=200)