from flask import request, current_app as app

from flask.views import View

from src import utils, checks


class ChangeUsername(View):

	@checks.login_check
	def dispatch_request(self, userid):

		data = request.get_json()

		username = data["newUsername"]

		app.mongo.db.userStats.update_one({"userId": userid}, {"$set": {"username": username}}, upsert=True)

		return "OK", 200
