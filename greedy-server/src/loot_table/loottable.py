from __future__ import annotations

from random import Random
from typing import Any, Optional

from .lootitem import LootItem
from .lootobject import LootObject


class LootTable(LootObject):

    def __init__(self):
        self._random: Optional[Random] = None

        self._all_drops: list[LootObject] = []
        self._avail_drops: Optional[list[LootObject]] = None

    def add_item(self, value: Any, weight: int = 1, unique: bool = False, always: bool = False):
        if isinstance(value, LootTable):
            value.update(weight, unique, always)
            self._all_drops.append(value)
        else:
            self._all_drops.append(LootItem(value, weight, unique, always))

    def add_items(self, values: list[Any], weight: int = 1, unique: bool = False, always: bool = False):
        for item in values:
            self.add_item(item, weight, unique, always)

    def get_items(self, count: int, rnd: Random) -> list[LootItem]:
        self._random = rnd

        if self._avail_drops is None:
            self._avail_drops = self._all_drops.copy()

        result_list: list[Any] = []

        # Add 'always' drops to the result
        for result in filter(lambda x: x.always, self._avail_drops):
            self._add_to_result(result, result_list)

        while count > len(result_list):
            if len(self._avail_drops) == 0:
                break

            self._add_new_item_to_result(self._avail_drops, result_list)

        return result_list

    def _add_new_item_to_result(self, droppables: list[LootObject], result_list: list[LootObject]):
        total_probability = sum(map(lambda x: x.weight, droppables))

        hit_value = self._random.uniform(0, total_probability)

        for i, drop in enumerate(droppables):
            hit_value -= drop.weight

            if hit_value <= 0:
                self._add_to_result(drop, result_list)
                break

    def _add_to_result(self, drop: LootObject, result_list: list[LootObject]):

        if drop.unique:
            self._avail_drops.remove(drop)

        if isinstance(drop, LootItem):
            result_list.append(drop.value)

        elif isinstance(drop, LootTable):
            result_list.extend(vals := drop.get_items(1, self._random))

            if len(vals) == 0 and drop in self._avail_drops:
                self._avail_drops.remove(drop)
