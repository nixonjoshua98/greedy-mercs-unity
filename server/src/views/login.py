from flask import Response, request

from flask.views import View

from src import utils


class Login(View):

	def __init__(self, mongo):

		self.mongo = mongo

	def dispatch_request(self):

		data = utils.decompress(request.data)

		# - New player (first login)
		if (row := self.mongo.db.userLogins.find_one({"deviceId": data["deviceId"]})) is None:
			result = self.mongo.db.userLogins.insert_one({"deviceId": data["deviceId"]})

			user_id = result.inserted_id

		else:
			user_id = row["_id"]

		items = self.mongo.db.userItems.find_one({"userId": user_id}) or dict()

		return Response(
			utils.compress(
				{"prestigePoints": items.get("prestigePoints", 0)}
			),

			status=200)

