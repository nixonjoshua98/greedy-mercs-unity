
import datetime as dt

from src.exts import mongo, resources

from src.classes.serverresponse import ServerResponse


class ItemType:
	FLAT_BLUE_GEM 	= 100
	FLAT_AP 		= 200  # Armoury Points


# = = = Public Methods = = =

def verify_purchase_ability(uid, iid) -> bool:
	return _within_purchase_limit(uid, iid) and _can_afford_purchase(uid, iid)


def process(uid, iid):
	try:
		return _perform_purchase(uid, iid)

	finally:
		_log_purchase(uid, iid)


# = = = Info Getter Methods = = =

def item_price(iid: int):
	item_data = resources.get("bountyshop")["items"][iid]

	return item_data["purchaseCost"]


def item_key(type_: int):
	return {
		ItemType.FLAT_BLUE_GEM: "blueGem",
		ItemType.FLAT_AP: "armouryPoint"
	}[type_]


# = = = Internal Methods = = =

def _perform_purchase(uid, iid):
	"""
	Performs the actual purchase and database update, assumes that the user (uid) can perform the purchase
	==================

	:param uid: Internal user ID
	:param iid: Item ID

	:return: Server response to be sent back to the client
	"""

	item_data = resources.get("bountyshop")["items"][iid]
	item_type = item_data["itemTypeId"]

	key = item_key(item_type)

	# Database Query Updates
	inc = {"bountyPoint": -item_price(iid), key: item_data["quantityPerPurchase"]}

	items = mongo.update_items(uid, inc=inc)

	return ServerResponse({"userItems": items})


def _log_purchase(uid, iid: int):
	now = dt.datetime.utcnow()

	mongo.db["bountyShopPurchases"].insert_one({"userId": uid, "purchaseTime": now, "itemId": iid})


def _within_purchase_limit(uid, iid: int) -> bool:
	item_data = resources.get("bountyshop")["items"][iid]

	user_purchases = mongo.db["bountyShopPurchases"].find(
		{"userId": uid, "itemId": iid, "purchaseTime": {"$gte": resources.last_reset()}}
	)

	user_purchases = list(user_purchases)

	return len(user_purchases) < item_data["dailyPurchaseLimit"]


def _can_afford_purchase(uid, iid: int):
	row = mongo.db["userItems"].find_one({"userId": uid})

	return (row is not None) and (row.get("bountyPoint", 0) > item_price(iid))
