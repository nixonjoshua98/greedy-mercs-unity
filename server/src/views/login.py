from flask import Response, request, current_app as app

from flask.views import View

from src import utils


class Login(View):

	def dispatch_request(self):

		data = utils.decompress(request.data)

		if (row := app.mongo.db.userLogins.find_one({"deviceId": data["deviceId"]})) is None:
			result = app.mongo.db.userLogins.insert_one({"deviceId": data["deviceId"]})

			uid = result.inserted_id

		else:
			uid = row["_id"]

		items = app.mongo.db.userItems.find_one({"userId": uid}) or dict()
		bounties = app.mongo.db.userBounties.find({"userId": uid}, {"_id": 0, "userId": 0})

		bounties = list(bounties)  # - Convert from a cursor to a list

		# - Convert date to timestamp so we can jsonify it
		for i, _ in enumerate(bounties):
			bounties[i]["startTime"] = bounties[i]["startTime"].timestamp() * 1000  # Convert to ms

		print(uid)

		return Response(
			utils.compress(
				{
					"weapons": items.get("weapons", dict()),
					"relics": items.get("relics", dict()),
					"bounties": bounties,
					"bountyPoints": items.get("bountyPoints", 0),
					"prestigePoints": str(items.get("prestigePoints", 0))
				},
			),

			status=200)