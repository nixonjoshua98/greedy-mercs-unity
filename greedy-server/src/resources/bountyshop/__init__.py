from __future__ import annotations

import datetime as dt
from random import Random
from typing import Union

from fastapi import Depends
from pydantic import Field

from src import utils
from src.pymodels import BaseModel
from src.request_context import RequestContext

from .loottable import BountyShopLootTable

from ..armoury import StaticArmouryItem, static_armoury
from .shopconfig import (FullBountyShopConfig, BountyShopLevelConfig,
                         bounty_shop_config, ArmouryItemsConfig)


async def dynamic_bounty_shop(
        ctx: RequestContext = Depends(),
        s_armoury: list[StaticArmouryItem] = Depends(static_armoury),
        shop_config: FullBountyShopConfig = Depends(bounty_shop_config),
) -> DynamicBountyShop:

    return DynamicBountyShop(s_armoury, ctx.prev_daily_reset, shop_config)


class BountyShopArmouryItem(BaseModel):
    id: str = Field(..., alias="itemId")

    armoury_item: int = Field(..., alias="armouryItem")
    purchase_cost: int = Field(..., alias="purchaseCost")
    purchase_limit: int = Field(1, alias="purchaseLimit")


class DynamicBountyShop:
    def __init__(
            self,
            s_armoury: list[StaticArmouryItem],
            prev_reset: dt.datetime,
            config: FullBountyShopConfig,
    ):
        self.prev_reset: dt.datetime = prev_reset
        self.config: BountyShopLevelConfig = self._get_shop_config(config)
        self.loot_Table = BountyShopLootTable(s_armoury, prev_reset, self.config)

        self.all_items = self._generate_shop()

    def get_item(self, item: str) -> Union[BountyShopArmouryItem]:
        return utils.get(self.all_items, id=item)

    def dict(self) -> dict[str, list]:
        return {
            "armouryItems": [x.client_dict() for x in self.all_items if isinstance(x, BountyShopArmouryItem)]
        }

    @staticmethod
    def _get_shop_config(config: FullBountyShopConfig):
        return config.get_level_config(0)

    def _generate_shop(self):
        rnd = Random(x=f"{(self.prev_reset - dt.datetime.fromtimestamp(0)).days}")

        shop_items = []

        items = self.loot_Table.get_items(self.config.num_items, rnd)

        # Fetch the items and iterate over them to create a bounty shop item variant of them
        for i, item in enumerate(items):

            if isinstance(item, StaticArmouryItem):
                shop_items.append(self._create_armoury_item(i, item))

        return shop_items

    @staticmethod
    def _create_armoury_item(idx: int, item: StaticArmouryItem):
        return BountyShopArmouryItem(id=f"AI-{item.id}-{idx}", armoury_item=item.id, purchase_cost=100)
