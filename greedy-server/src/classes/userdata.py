
from src.common import mongo

from src import dbutils


class CompleteUserData:
    def __init__(self, uid):
        self.uid = uid

        self.armoury = UserArmouryData(uid)
        self.bounty_shop = BountyShopData(uid)


class UserArmouryData:
    def __init__(self, uid):
        self.uid = uid

    def as_list(self):
        return list(mongo.db["userArmouryItems"].find({"userId": self.uid}))

    def update(self, iid: int, inc_: dict, *, upsert: bool = False):
        mongo.db["userArmouryItems"].update_one(
            {"userId": self.uid, "itemId": iid},
            {"$inc": inc_},
            upsert=upsert
        )


class BountyShopData:
    def __init__(self, uid):
        self.uid = uid

    def daily_purchases(self, *, iid: int = None):

        filter_ = {"userId": self.uid, "purchaseTime": {"$gte": dbutils.last_daily_reset()}}

        if iid is not None:
            filter_["itemId"] = iid

        results = list(mongo.db["bountyShopPurchases"].find(filter_))

        def count(item_id: int):
            return len([r for r in results if r["itemId"] == item_id])

        data = {iid_: count(iid_) for iid_ in set(r["itemId"] for r in results)}

        return data.get(iid, 0) if iid is not None else data
