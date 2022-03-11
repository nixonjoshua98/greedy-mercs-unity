from __future__ import annotations

import datetime as dt
from random import Random

from fastapi import Depends

from src import utils
from src.auth import RequestContext
from src.dependencies import get_static_armoury
from src.static_models.armoury import StaticArmouryItem

from .loottable import BountyShopLootTable
from .models import (BountyShopArmouryItem, BountyShopCurrencyItem,
                     PurchasableBountyShopItem)
from .shopconfig import (BountyShopLevelConfig, CurrencyItemConfig,
                         FullBountyShopConfig, bounty_shop_config)


async def dynamic_bounty_shop(
    ctx: RequestContext = Depends(),
    static_data: list[StaticArmouryItem] = Depends(get_static_armoury),
    shop_config: FullBountyShopConfig = Depends(bounty_shop_config),
) -> DynamicBountyShop:

    return DynamicBountyShop(static_data, ctx.prev_daily_reset, shop_config)


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

        self.all_items: list[PurchasableBountyShopItem] = self._generate_shop()

    def get_item(self, item: str):
        return utils.get(self.all_items, id=item)

    def dict(self) -> dict[str, list]:
        return {
            "armouryItems": [x.dict() for x in self.all_items if isinstance(x, BountyShopArmouryItem)],
            "currencyItems": [x.dict() for x in self.all_items if isinstance(x, BountyShopCurrencyItem)]
        }

    @staticmethod
    def _get_shop_config(config: FullBountyShopConfig):
        return config.get_level_config(0)

    def _generate_shop(self) -> list[PurchasableBountyShopItem]:
        rnd = Random(x=f"{(self.prev_reset - dt.datetime.fromtimestamp(0)).days}")

        shop_items = []

        items = self.loot_Table.get_items(5, rnd)

        # Fetch the items and iterate over them to create a bounty shop item variant of them
        for i, item in enumerate(items):

            if isinstance(item, StaticArmouryItem):
                shop_items.append(self._armoury_item(i, item))

            elif isinstance(item, CurrencyItemConfig):
                shop_items.append(self._currency_item(i, item))

        return shop_items

    @staticmethod
    def _armoury_item(idx: int, item: StaticArmouryItem) -> BountyShopArmouryItem:
        return BountyShopArmouryItem(
            id=f"AI-{idx}",
            armoury_item_id=item.id,
            purchase_cost=100
        )

    @staticmethod
    def _currency_item(idx: int, item: CurrencyItemConfig) -> BountyShopCurrencyItem:
        return BountyShopCurrencyItem(
            id=f"CI-{idx}",
            purchase_quantity=item.purchase_quantity,
            currency_type=item.currency_type,
            purchase_cost=50
        )
