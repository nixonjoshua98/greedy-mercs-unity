from fastapi import Depends

from src.file_cache import StaticFilesCache, get_static_files_cache
from src.shared_models import BaseModel, Field


def bounty_shop_config(cache: StaticFilesCache = Depends(get_static_files_cache)):
    return FullBountyShopConfig.parse_obj(cache.load_file("server/bountyshop.json5"))


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
