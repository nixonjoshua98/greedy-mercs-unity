from flask import current_app as app

from flask.views import View

from src import checks


class ResetAccount(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		app.mongo.db.userItems.update_one({"userId": userid}, {"$set": {"relics": {}, "weapons": {}}})

		return "OK", 200
