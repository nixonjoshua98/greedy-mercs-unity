from __future__ import annotations

from src.pymodels import BaseModel, Field
from src.routing import ServerRequest


def bounty_shop_config(request: ServerRequest):
    return FullBountyShopConfig.parse_obj(request.app.get_static_file("server/bountyshop.json"))


# = Armoury Items = #

class ArmouryItemsConfig(BaseModel):
    weight: int


# = Currency Items = #

class CurrencyItemConfig(BaseModel):
    item_id: int = Field(..., alias="ItemId")


class CurrencyItemsConfig(BaseModel):
    weight: int
    items: list[CurrencyItemConfig]


# = Default = #

class BountyShopLevelConfig(BaseModel):
    num_items: int = 5

    armoury_items: ArmouryItemsConfig = Field(..., alias="armouryItems")


class FullBountyShopConfig(BaseModel):
    level0: BountyShopLevelConfig = Field(..., alias="level-0")

    def get_level_config(self, lvl: int) -> BountyShopLevelConfig:
        return getattr(self, f"level{lvl}")
