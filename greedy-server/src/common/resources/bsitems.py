
from src.common.enums import EnumBase


class ItemType(EnumBase):
	FLAT_BLUE_GEMS = 100
	FLAT_AP = 200


class SvrBountyShopData:
	def __init__(self, data):
		self.dict = data

		self.normal_items: dict = {k: BountyShopItem(k, v) for k, v in data["items"].items()}
		self.armoury_items: dict = {k: BsArmouryItem(k, v) for k, v in data["armouryItems"].items()}


class BsShopItemBase:
	def __init__(self, id_: int, data: dict):
		self.id = id_

		self.purchase_cost 			= data["purchaseCost"]
		self.daily_purchase_limit 	= data["dailyPurchaseLimit"]
		self.quantity_per_purchase 	= data["quantityPerPurchase"]


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