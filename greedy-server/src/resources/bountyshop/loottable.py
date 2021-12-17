from __future__ import annotations

import datetime as dt
from random import Random

from src.loot_table import LootTable

from ..armoury import StaticArmouryItem
from .shopconfig import BountyShopLevelConfig, CurrencyItemsConfig, ArmouryItemsConfig


class BountyShopLootTable:
    def __init__(
            self,
            s_armoury: list[StaticArmouryItem],
            prev_reset: dt.datetime,
            config: BountyShopLevelConfig
    ):
        self.prev_reset: dt.datetime = prev_reset
        self.config: BountyShopLevelConfig = config
        self.s_armoury: list[StaticArmouryItem] = s_armoury

        self._table: LootTable = self._create_loot_table()

    def get_items(self, count: int, rnd: Random):
        return self._table.get_items(count, rnd)

    def _create_loot_table(self) -> LootTable:

        root = LootTable()

        self._add_armoury_items_table(root, self.config.armoury_items)
        self._add_currency_items_table(root, self.config.currency_items)

        return root

    def _add_armoury_items_table(self, root: LootTable, config: ArmouryItemsConfig):
        for item in self.s_armoury:
            root.add_item(item, weight=config.weight)

    @staticmethod
    def _add_currency_items_table(root: LootTable, config: CurrencyItemsConfig):
        for item in config.items:
            root.add_item(item, weight=config.weight, always=item.always)
