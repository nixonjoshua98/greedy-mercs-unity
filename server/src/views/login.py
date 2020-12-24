from flask import request

from flask.views import View

from src import utils


class Login(View):

	def __init__(self, mongo):

		self.mongo = mongo

	def dispatch_request(self):

		data = utils.decompress(request.data)

		# - New player (first login)
		if (row := self.mongo.db.userLogins.find_one({"deviceId": data["deviceId"]})) is None:
			self.mongo.db.userLogins.insert_one({"deviceId": data["deviceId"]})

		return "OK"
