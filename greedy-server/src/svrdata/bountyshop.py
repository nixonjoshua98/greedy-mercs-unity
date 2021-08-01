import random

import datetime as dt

from src import svrdata
from src.common import mongo, resources
from src.common.enums import ItemType


def daily_purchases(uid, iid: int = None):
    """ Count the number of purchase made for an item (if provided) by a user since the previous reset. """
    filter_ = {"userId": uid, "purchaseTime": {"$gte": svrdata.last_daily_reset()}}

    if iid is not None:
        filter_["itemId"] = iid

    results = list(mongo.db["bountyShopPurchases"].find(filter_))

    def count(item_id: int):
        return len([r for r in results if r["itemId"] == item_id])

    data = {iid_: count(iid_) for iid_ in set(r["itemId"] for r in results)}

    return data.get(iid, 0) if iid is not None else data


def current_items() -> dict[str, "BsItem"]:
    """ Return a 'dict' of the current normal shop items. """
    return {k: BsItem(k, v) for k, v in resources.get("bountyshopitems")["items"].items()}


def current_armoury_items() -> dict:
    """ Return a 'dict' of the current armoury shop items. """
    last_reset = svrdata.last_daily_reset()

    with RandomContext(last_reset.timestamp()):
        return _generate_armoury_items()


def all_current_shop_items(*, as_dict: bool):
    d = {"items": current_items(), "armouryItems": current_armoury_items()}

    if as_dict:
        d = {k: {k2: v2.as_dict() for k2, v2 in v.items()} for k, v in d.items()}

    return d


class BsShopItemBase:
    def __init__(self, id_: str, data: dict):
        self._dict = data

        self.id: str = id_

        self.purchase_cost: int = data["purchaseCost"]
        self.daily_purchase_limit: int = data["dailyPurchaseLimit"]

    def as_dict(self):
        return self._dict


class BsItem(BsShopItemBase):
    def __init__(self, id_: str, data: dict):
        super(BsItem, self).__init__(id_, data)

        self.item_type: ItemType = ItemType.get_val(data["itemType"])

        self.quantity_per_purchase = data["quantityPerPurchase"]


class BsArmouryItem(BsShopItemBase):
    def __init__(self, id_: str, data: dict):
        super(BsArmouryItem, self).__init__(id_, data)

        self.armoury_item: int = data["armouryItemId"]


# # # Shop Generation # # #

def _generate_armoury_items() -> dict:
    last_reset = svrdata.last_daily_reset()

    all_items, selected_items = resources.get(resources.ARMOURY)["items"], {}

    keys = random.choices(list(all_items.keys()), k=9)

    days_since_epoch = (last_reset - dt.datetime.fromtimestamp(0)).days

    for i, key in enumerate(keys):
        item_data = all_items[key]

        _id = f"AI-{days_since_epoch}-{key}-{i}"

        entry = {
            "armouryItemId": key,
            "purchaseCost": 100 + (item_data["itemTier"] * 125),
            "dailyPurchaseLimit": max(1, 3 - (item_data["itemTier"] - 1)),
        }

        selected_items[_id] = BsArmouryItem(_id, entry)

    return selected_items


class RandomContext:
    def __init__(self, seed):
        self._seed = seed

        self._prev_state = random.getstate()

    def __enter__(self):
        random.seed(f"{self._seed}")

    def __exit__(self, exc_type, exc_val, exc_tb):
        random.setstate(self._prev_state)
