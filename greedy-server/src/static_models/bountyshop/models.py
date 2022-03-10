from __future__ import annotations

from src.pymodels import BaseModel, Field


class PurchasableBountyShopItem(BaseModel):
    id: str = Field(..., alias="itemId")

    purchase_cost: int = Field(..., alias="purchaseCost")
    purchase_limit: int = Field(1, alias="purchaseLimit")


class BountyShopArmouryItem(PurchasableBountyShopItem):
    armoury_item_id: int = Field(..., alias="armouryItemId")


class BountyShopCurrencyItem(PurchasableBountyShopItem):
    currency_type: int = Field(..., alias="currencyType")
    purchase_quantity: int = Field(..., alias="quantityPerPurchase")
