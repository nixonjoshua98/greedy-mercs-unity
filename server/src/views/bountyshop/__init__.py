
from flask import request
from flask.views import View

from src import dbutils

from src import checks
from src.exts import mongo, resources
from src.classes.serverresponse import ServerResponse

from . import shop_items


class BountyShop(View):

	@checks.login_check
	def dispatch_request(self, *, uid, data):
		purpose = request.args.get("purpose")

		if purpose == "refreshShop":
			return self.refresh_shop(uid, data)

		elif purpose == "purchaseItem":
			return self.purchase_item(uid, data)

		return "400", 400

	def refresh_shop(self, uid, data):
		return ServerResponse(
			{
				"serverData": resources.get("bountyshop"),
				"dailyPurchases": dbutils.bs.get_daily_purchases(uid)
			}
		)

	def purchase_item(self, uid, data):
		iid = data["itemId"]

		if not shop_items.verify_purchase_ability(uid, iid):
			return "400", 400

		return shop_items.process(uid, iid)
