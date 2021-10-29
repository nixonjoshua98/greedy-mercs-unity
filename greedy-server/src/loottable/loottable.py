from __future__ import annotations

from collections import defaultdict
from random import Random
from typing import Any, Union

from .lootitem import LootItem


class LootTable:
    def __init__(self):
        self._items = []

    def add_item(self, item: Any, *, weight: int = 1, unique: bool = False):
        self._items.append(LootItem(item=item, weight=weight, unique=unique))

    def get_items(self, count: int, rnd: Random):
        top_level_drops = self._get_top_level_drops(count, rnd)

        drops = []

        for item, count in top_level_drops.items():
            actual_item = item.item

            if isinstance(actual_item, LootTable):
                drops.extend(actual_item.get_items(count, rnd))
            else:
                drops.extend([actual_item for _ in range(count)])

        return drops

    def _get_top_level_drops(
        self, count: int, rnd: Random
    ) -> dict[Union[LootItem, LootTable], int]:
        droppables = self._items.copy()

        dropped_items = defaultdict(int)

        while droppables and sum(dropped_items.values()) < count:
            item: Union[LootTable, LootItem] = self._get_next_item(droppables, rnd)

            dropped_items[item] += 1

            if item.unique:
                droppables.remove(item)

        return dropped_items

    @staticmethod
    def _total_weight(items: list[LootItem]):
        return sum(i.weight for i in items)

    @classmethod
    def _get_next_item(
        cls, items: list[LootItem], rnd: Random
    ) -> Union[LootTable, LootItem]:
        hit_value = rnd.uniform(0, cls._total_weight(items))

        running_value = 0
        for item in items:
            running_value += item.weight
            if running_value >= hit_value:
                return item
