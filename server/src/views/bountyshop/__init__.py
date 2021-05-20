from flask import request

from flask.views import View

from src import checks

from src.exts import mongo

from src.classes import resources

from src.classes.serverresponse import ServerResponse


class BountyShop(View):

	@checks.login_check
	def dispatch_request(self, *, uid, data):
		purpose = request.args.get("purpose")

		if purpose == "refreshShop":
			return self.refresh_shop(uid, data)

		elif purpose == "purchaseItem":
			return self.purchase_item(uid, data)

		return "400", 400

	def purchase_item(self, uid, data):
		items = mongo.update_items(uid, inc_={"bountyPoints": 1})

		return ServerResponse({"userItems": items})

	def refresh_shop(self, uid, data):
		server_data = resources.get("bountyshop")

		return ServerResponse({"serverData": server_data})
