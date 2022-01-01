from typing import Any

from .lootobject import LootObject


class LootItem(LootObject):
    def __init__(self, value: Any, weight: int, unique: bool, always: bool):
        self.update(weight, unique, always)

        self.value: Any = value
