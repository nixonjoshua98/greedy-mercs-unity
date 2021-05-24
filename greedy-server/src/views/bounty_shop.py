
from flask import request
from flask.views import View

import datetime as dt

from src import dynamicdata

from src.common import mongo, checks, dbutils
from src.classes import ServerResponse


class BountyShop(View):

	@checks.login_check
	def dispatch_request(self, *, uid, data):
		purpose = request.args.get("purpose")

		if purpose == "refreshShop":
			return self.refresh_shop(uid, data)

		elif purpose == "purchaseItem":
			return self.purchase_item(uid, data)

		elif purpose == "purchaseArmouryItem":
			return self.purchase_armoury_item(uid, data)

		return "400", 400

	def refresh_shop(self, uid, _):
		shop_items = dynamicdata.get_bounty_shop(uid)

		return ServerResponse(
			{
				"serverData": shop_items.to_dict(),
				"dailyPurchases": dbutils.bs.get_daily_purchases(uid),
				"nextDailyResetTime": dbutils.next_daily_reset()
			}
		)

	def purchase_item(self, uid, data):
		shop_items = dynamicdata.get_bounty_shop(uid)

		item = shop_items.normal_items.get((iid := data["itemId"]))

		if item is None or not self._can_purchase_item(uid, item=item):
			return "400", 400

		try:
			items = dbutils.inventory.update_items(uid, inc={
					"bountyPoints": -item.purchase_cost,
					item.get_db_key(): item.quantity_per_purchase
				}
			)
			return ServerResponse({"userItems": items})

		finally:
			self._log_purchase(uid, iid)

	def purchase_armoury_item(self, uid, data):
		shop_items = dynamicdata.get_bounty_shop(uid)

		item = shop_items.armoury_items.get((iid := data["itemId"]))

		if item is None or not self._can_purchase_item(uid, item=item):
			return "400", 400

		try:
			armoury = dbutils.armoury.update_item(uid, item.armoury_item_id, inc={"owned": item.quantity_per_purchase})

			items = dbutils.inventory.update_items(uid, inc={"bountyPoints": -item.purchase_cost})

			return ServerResponse({"userItems": items, "userArmouryItems": armoury})

		finally:
			self._log_purchase(uid, iid)

	@staticmethod
	def _can_purchase_item(uid, *, item):

		# Get the num. of purchase for this item since last reset
		num_purchases = dbutils.bs.get_daily_purchases(uid, iid=item.id)

		# Grab the users items data
		user_items = dbutils.inventory.get_items(uid)

		return (item.daily_purchase_limit > num_purchases) and (user_items.get("bountyPoints", 0) > item.purchase_cost)

	@staticmethod
	def _log_purchase(uid, iid: int):
		mongo.db["bountyShopPurchases"].insert_one({"userId": uid, "purchaseTime": dt.datetime.utcnow(), "itemId": iid})
