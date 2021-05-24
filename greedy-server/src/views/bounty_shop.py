
from flask import request
from flask.views import View

import datetime as dt

from src.common import mongo, checks, dbutils
from src.classes import ServerResponse

from src.common.resources import res
from src.common.resources.bsitems import BsShopItemBase, BsArmouryItem


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
		return ServerResponse(
			{
				"serverData": res.bounty_shop.dict,
				"dailyPurchases": dbutils.bs.get_daily_purchases(uid),
				"nextDailyResetTime": dbutils.next_daily_reset()
			}
		)

	def purchase_item(self, uid, data):
		item = res.bounty_shop.normal_items.get((iid := data["itemId"]))

		if not self._can_purchase_item(uid, item=item):
			return "400", 400

		try:
			items = dbutils.inv.update_items(uid, inc={
					"bountyPoints": -item.purchase_cost,
					item.get_db_key(): item.quantity_per_purchase
				}
			)
			return ServerResponse({"userItems": items})

		finally:
			self._log_purchase(uid, iid)

	def purchase_armoury_item(self, uid, data):
		item: BsArmouryItem = res.bounty_shop.armoury_items.get((iid := data["itemId"]))

		if not self._can_purchase_item(uid, item=item):
			return "400", 400

		try:
			armoury = dbutils.armoury.update_item(uid, item.armoury_item_id, inc={"owned": item.quantity_per_purchase})

			items = dbutils.inv.update_items(uid, inc={"bountyPoints": -item.purchase_cost})

			return ServerResponse({"userItems": items, "userArmouryItems": armoury})

		finally:
			self._log_purchase(uid, iid)

	@staticmethod
	def _can_purchase_item(uid, *, item: BsShopItemBase):

		# Get the num. of purchase for this item since last reset
		num_purchases = dbutils.bs.get_daily_purchases(uid, iid=item.id)

		# Grab the users items data
		user_items = dbutils.inv.get_items(uid)

		return (item.daily_purchase_limit > num_purchases) and (user_items.get("bountyPoints", 0) > item.purchase_cost)

	@staticmethod
	def _log_purchase(uid, iid: int):
		mongo.db["bountyShopPurchases"].insert_one({"userId": uid, "purchaseTime": dt.datetime.utcnow(), "itemId": iid})
