import datetime as dt

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

		items = app.mongo.db.userItems.find_one({"userId": uid}, {"_id": 0, "userId": 0}) or dict()
		bounties = app.mongo.db.userBounties.find_one({"userId": uid}, {"_id": 0, "userId": 0})

		if bounties is None:
			now = dt.datetime.utcnow()

			app.mongo.db.userBounties.update_one({"userId": uid}, {"$set": {"lastClaimTime": now}}, upsert=True)

			bounties = {"lastClaimTime": now}

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