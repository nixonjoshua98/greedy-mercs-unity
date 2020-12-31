from flask import current_app as app

from flask.views import View

from src import checks


class ResetRelics(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		app.mongo.db.userItems.update_one({"userId": userid}, {"$set": {"relics": []}}) or dict()

		return "OK", 200
