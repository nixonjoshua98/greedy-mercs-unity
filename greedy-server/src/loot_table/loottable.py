from __future__ import annotations

from random import Random
from typing import Any, Optional

from .lootitem import LootItem


class LootTable:

    def __init__(self, items: list[Any] = None):
        self._random: Optional[Random] = None

        self._all_drops: list[LootItem] = []
        self._unique_drops: list[LootItem] = []

        if isinstance(items, list):
            self.add_items(items)

    def add_item(self, value: Any, weight: int = 1, unique: bool = False, always: bool = False):
        self._all_drops.append(LootItem(value, weight, unique, always))

    def add_items(self, values: list[Any], weight: int = 1, unique: bool = False, always: bool = False):
        for item in values:
            self.add_item(item, weight, unique, always)

    def get_items(self, count: int, rnd: Random) -> list[LootItem]:
        self._random = rnd

        result_list: list[Any] = []

        # Add 'always' drops to the result
        for result in filter(lambda x: x.always, self._all_drops):
            self._add_to_result(result, result_list)

        remaining_count = count - len(result_list)

        for _ in range(remaining_count):
            # Remove already dropped unique items
            droppables = list(filter(lambda x: not x.unique or x not in self._unique_drops, self._all_drops))

            if len(droppables) > 0:
                self._add_new_item_to_result(droppables, result_list)

        return result_list

    def _add_new_item_to_result(self, droppables: list[LootItem], result_list: list[LootItem]):
        total_probability = sum(map(lambda x: x.weight, droppables))

        hit_value = self._random.uniform(1, total_probability)

        for i, drop in enumerate(droppables):
            hit_value -= drop.weight

            if hit_value <= 0:
                self._add_to_result(drop, result_list)
                break

    def _add_to_result(self, drop: LootItem, result_list: list[LootItem]):

        if drop.unique:
            self._unique_drops.append(drop)

        result_list.append(drop.value)
