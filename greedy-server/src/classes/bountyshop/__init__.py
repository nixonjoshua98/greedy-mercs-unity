import random

import datetime as dt

from src import dbutils
from src.common import resources
from src.common.enums import EnumBase


class ItemType(EnumBase):
    BLUE_GEMS = 100
    ARMOURY_POINTS = 200
    PRESTIGE_POINTS = 300


class BountyShopItems:
    def __init__(self, uid):
        self.normal_items = dict()
        self.armoury_items = dict()

        self._generate_shop(uid)

    def _generate_shop(self, uid):
        last_reset = dbutils.last_daily_reset()

        state = random.getstate()

        random.seed(f"{last_reset.timestamp()}")

        try:
            self.normal_items = self._generate_normal_items(uid)
            self.armoury_items = self._generate_armoury_items(uid)

        finally:
            random.setstate(state)

    @staticmethod
    def _generate_normal_items(uid) -> dict:
        return {k: BountyShopItem(k, v) for k, v in resources.get("bountyshopitems")["items"].items()}

    @staticmethod
    def _generate_armoury_items(uid) -> dict:
        last_reset = dbutils.last_daily_reset()

        all_items, selected_items = resources.get("armouryitems"), {}

        keys = random.choices(list(all_items.keys()), k=9)

        days_since_epoch = (last_reset - dt.datetime.fromtimestamp(0)).days

        for i, key in enumerate(keys):
            item_data = all_items[key]

            id_ = (days_since_epoch * 10_000) - (i * 1_000) - (key * 100)

            entry = {
                "armouryItemId": key,
                "purchaseCost": 100 + (item_data["itemTier"] * 125),
                "dailyPurchaseLimit": max(1, 3 - (item_data["itemTier"] - 1)),
                "quantityPerPurchase": 1
            }

            selected_items[id_] = BsArmouryItem(id_, entry)

        return selected_items

    def to_dict(self):
        return {
            "items": {k: v.as_dict() for k, v in self.normal_items.items()},
            "armouryItems": {k: v.as_dict() for k, v in self.armoury_items.items()}
        }


class BsShopItemBase:
    def __init__(self, id_: int, data: dict):
        self._dict = data

        self.id = id_

        self.purchase_cost = data["purchaseCost"]
        self.daily_purchase_limit = data["dailyPurchaseLimit"]
        self.quantity_per_purchase = data["quantityPerPurchase"]

    def to_dict(self):
        return self._dict


class BountyShopItem(BsShopItemBase):
    def __init__(self, id_: int, data: dict):
        super(BountyShopItem, self).__init__(id_, data)

        self.item_type = ItemType.get_val(data["itemType"])

    def get_db_key(self):
        return {
            ItemType.BLUE_GEMS: "blueGems",
            ItemType.ARMOURY_POINTS: "armouryPoints",
            ItemType.PRESTIGE_POINTS: "prestigePoints"
        }[self.item_type]


class BsArmouryItem(BsShopItemBase):
    def __init__(self, id_: int, data: dict):
        super(BsArmouryItem, self).__init__(id_, data)

        self.armoury_item_id = data["armouryItemId"]
