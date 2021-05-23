
from flask import request
from flask.views import View

import datetime as dt

from src.common import mongo, resources, checks, dbutils
from src.classes import ServerResponse


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
				"dailyPurchases": dbutils.bs.get_daily_purchases(uid),
				"nextDailyResetTime": dbutils.next_daily_reset()
			}
		)

	def purchase_item(self, uid, data):
		# Load resource file
		item_data = resources.get("bountyshop")["items"][(iid := data["itemId"])]

		max_purchases = item_data["dailyPurchaseLimit"]
		num_purchases = dbutils.bs.get_daily_purchases(uid, iid=iid)

		user_items = dbutils.inv.get_items(uid)

		cost = self._get_purchase_cost(iid)  # Purchase cost for this item

		# Check if the user can actually buy the item
		if (num_purchases >= max_purchases) or (cost > user_items.get("bountyPoints", 0)):
			return "400", 400

		try:
			key = {100: "blueGems", 200: "armouryPoints"}[item_data["itemType"]]

			items = dbutils.inv.update_items(uid, inc={"bountyPoints": -cost, key: item_data["quantityPerPurchase"]})

			return ServerResponse({"userItems": items})

		finally:
			self._log_purchase(uid, iid)

	@staticmethod
	def _get_purchase_cost(iid):
		item_data = resources.get("bountyshop")["items"][iid]

		return item_data["purchaseCost"]

	@staticmethod
	def _log_purchase(uid, iid: int):
		now = dt.datetime.utcnow()

		mongo.db["bountyShopPurchases"].insert_one({"userId": uid, "purchaseTime": now, "itemId": iid})
