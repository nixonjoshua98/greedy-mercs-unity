
from src.common import mongo
from src import dbutils


def get_daily_purchases(uid, *, iid: int = None):
	filter_ = {"userId": uid, "purchaseTime": {"$gte": dbutils.last_daily_reset()}}

	if iid is not None:
		filter_["itemId"] = iid

	results = list(mongo.db["bountyShopPurchases"].find(filter_))

	count_func = lambda iid_: len([r for r in results if r["itemId"] == iid_])

	data = {iid_: count_func(iid_) for iid_ in set(r["itemId"] for r in results)}

	return data.get(iid, 0) if iid is not None else data
