from src.classes.randomcontext import RandomContext
from src.common.enums import ItemType

import random

import datetime as dt
from src.common import resources
from src.dataloader.serverstate import ServerState


class BountyShopGeneration:
    def __init__(self, uid):
        self._uid = uid

        self.items = None
        self.armoury_items = None

        self.__generate()

    def get_item(self, iid):
        for d in (self.items, self.armoury_items):
            if item := d.get(iid):
                return item

    def to_dict(self) -> dict:
        return {
            "items": {k: v.to_dict() for k, v in self.items.items()},
            "armouryItems": {k: v.to_dict() for k, v in self.armoury_items.items()}
        }

    def __generate(self):
        state = ServerState()

        days_since_epoch = (state.prev_daily_reset - dt.datetime.fromtimestamp(0)).days

        with RandomContext(f"{days_since_epoch}"):
            self.items = self.__generate_currency_items()
            self.armoury_items = self.__generate_armoury_items(server_state=state)

    @staticmethod
    def __generate_currency_items():
        return {
            "ITEM-0": BountyShopCurrencyItem.create_from_params(
                "ITEM-0", ItemType.BLUE_GEMS, 25, 10
            ),
            "ITEM-1": BountyShopCurrencyItem.create_from_params(
                "ITEM-1", ItemType.ARMOURY_POINTS, 50, 10
            ),
            "ITEM-2": BountyShopCurrencyItem.create_from_params(
                "ITEM-2", ItemType.ARMOURY_POINTS, 75, 10
            )
        }

    @staticmethod
    def __generate_armoury_items(*, server_state):
        days_since_epoch = (server_state.prev_daily_reset() - dt.datetime.fromtimestamp(0)).days

        all_items, generated_items = resources.get(resources.ARMOURY)["items"], {}

        keys = random.choices(list(all_items.keys()), k=9)

        for i, key in enumerate(keys):
            _id = f"AI-{days_since_epoch}-{key}-{i}"

            generated_items[_id] = BountyShopArmouryItem.create_from_params(_id, key, 100)

        return generated_items


class AbstractBountyShopItem:
    def __init__(self, id_: str, data: dict):
        self._dict = data

        self.id: str = id_

        self.purchase_cost: int = data["purchaseCost"]

        self.daily_purchase_limit: int = data.get("dailyPurchaseLimit", 1)

        if data.get("dailyPurchaseLimit") is None:
            self._dict["dailyPurchaseLimit"] = self.daily_purchase_limit

    def to_dict(self):
        return self._dict


class BountyShopCurrencyItem(AbstractBountyShopItem):
    def __init__(self, id_: str, data: dict):
        super(BountyShopCurrencyItem, self).__init__(id_, data)

        self.item_type: ItemType = ItemType.get_val(data["itemType"])

        self.quantity_per_purchase: int = data["quantityPerPurchase"]

    def to_dict(self):
        d = super(BountyShopCurrencyItem, self).to_dict()

        d["itemType"] = d["itemType"].value

        return d

    @classmethod
    def create_from_params(cls, id_, type_: int, cost: int, quantity: int):
        return cls(id_, {
            "itemType": type_, "purchaseCost": cost, "quantityPerPurchase": quantity
        })


class BountyShopArmouryItem(AbstractBountyShopItem):
    def __init__(self, id_: str, data: dict):
        super(BountyShopArmouryItem, self).__init__(id_, data)

        self.armoury_item: int = data["armouryItemId"]

    @classmethod
    def create_from_params(cls, id_, armoury_item: int, cost: int):
        return cls(id_, {
            "armouryItemId": armoury_item, "purchaseCost": cost
        })
