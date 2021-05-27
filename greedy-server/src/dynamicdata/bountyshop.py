import random

from src.common import resources, dbutils


from src.common.enums import EnumBase


def get_bounty_shop(uid) -> "BountyShopItems":
	return BountyShopItems(uid)


class ItemType(EnumBase):
	FLAT_BLUE_GEMS = 100
	FLAT_AP = 200


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
			self.normal_items = _generate_normal_items(uid)
			self.armoury_items = _generate_armoury_items(uid)

		finally:
			random.setstate(state)

	def to_dict(self):
		return {
			"items": {k: v.to_dict() for k, v in self.normal_items.items()},
			"armouryItems": {k: v.to_dict() for k, v in self.armoury_items.items()}
		}


class BsShopItemBase:
	def __init__(self, id_: int, data: dict):
		self._dict = data

		self.id = id_

		self.purchase_cost 			= data["purchaseCost"]
		self.daily_purchase_limit 	= data["dailyPurchaseLimit"]
		self.quantity_per_purchase 	= data["quantityPerPurchase"]

	def to_dict(self):
		return self._dict


class BountyShopItem(BsShopItemBase):
	def __init__(self, id_: int, data: dict):
		super(BountyShopItem, self).__init__(id_, data)

		self.item_type = ItemType.get_val(data["itemType"])

	def get_db_key(self):
		return {ItemType.FLAT_BLUE_GEMS: "blueGems", ItemType.FLAT_AP: "armouryPoints"}[self.item_type]


class BsArmouryItem(BsShopItemBase):
	def __init__(self, id_: int, data: dict):
		super(BsArmouryItem, self).__init__(id_, data)

		self.armoury_item_id = data["armouryItemId"]


def _generate_normal_items(uid) -> dict:
	return {k: BountyShopItem(k, v) for k, v in resources.get("bountyshopitems")["items"].items()}


def _generate_armoury_items(uid) -> dict:
	last_reset = dbutils.last_daily_reset()

	all_items, selected_items = resources.get("armouryitems"), {}

	keys = random.choices(list(all_items.keys()), k=5)

	for i, key in enumerate(keys):
		item_data = all_items[key]

		id_ = 2_000 + key + i + last_reset.day

		entry = {
			"armouryItemId": key,
			"purchaseCost": 50 * (item_data["itemTier"] + 1),
			"dailyPurchaseLimit": max(1, 6 - item_data["itemTier"]),
			"quantityPerPurchase": 1
		}

		selected_items[id_] = BsArmouryItem(id_, entry)

	return selected_items
