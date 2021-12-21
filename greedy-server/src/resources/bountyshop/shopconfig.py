from __future__ import annotations

from src.pymodels import BaseModel, Field
from src.routing import ServerRequest


def bounty_shop_config(request: ServerRequest):
    return FullBountyShopConfig.parse_obj(request.app.get_static_file("server/bountyshop.json5"))


# = Armoury Items = #

class ArmouryItemsConfig(BaseModel):
    weight: int


# = Currency Items = #

class CurrencyItemConfig(BaseModel):
    unique: bool = Field(False)
    currency_type: int = Field(..., alias="currencyType")
    purchase_quantity: int = Field(..., alias="quantityPerPurchase")


class CurrencyItemsConfig(BaseModel):
    unique: bool = Field(False)
    always: bool = Field(False)
    weight: int = Field(1)
    items: list[CurrencyItemConfig]


# = Default = #

class BountyShopLevelConfig(BaseModel):
    currency_items: CurrencyItemsConfig = Field(..., alias="currencyItems")
    armoury_items: ArmouryItemsConfig = Field(..., alias="armouryItems")


class FullBountyShopConfig(BaseModel):
    level0: BountyShopLevelConfig = Field(..., alias="level-0")

    def get_level_config(self, lvl: int) -> BountyShopLevelConfig:
        return getattr(self, f"level{lvl}")
