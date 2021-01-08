from flask import current_app as app

from flask.views import View

from src import checks


class ResetAccount(View):

	@checks.login_check
	def dispatch_request(self, *, userid):

		app.mongo.db.userItems.delete_one({"userId": userid})

		app.mongo.db.userStats.delete_one({"userId": userid})

		app.mongo.db.userBounties.delete_one({"userId": userid})

		return "OK", 200
