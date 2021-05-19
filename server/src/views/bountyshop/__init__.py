from flask import request

from flask.views import View

from src import checks

from src.classes.serverresponse import ServerResponse


class BountyShop(View):

	@checks.login_check
	def dispatch_request(self, *, uid, data):
		purpose = request.args.get("purpose")

		if purpose == "buyItem":
			return self.buy_item(uid, data)

		return "400", 400

	def buy_item(self, uid, data):
		return ServerResponse({})
