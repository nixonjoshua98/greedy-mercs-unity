
from src.exts import mongo, resources


def get_daily_purchases(uid):
	cursor = mongo.db["bountyShopPurchases"].find({"userId": uid, "purchaseTime": {"$gte": resources.last_reset()}})

	results = list(cursor)

	count_func = lambda iid_: len([r for r in results if r["itemId"] == iid_])

	return {iid: count_func(iid) for iid in set(r["itemId"] for r in results)}
