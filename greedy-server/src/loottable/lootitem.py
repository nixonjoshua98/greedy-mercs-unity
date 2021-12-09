import dataclasses
from typing import Any


@dataclasses.dataclass(frozen=True, unsafe_hash=True)
class LootItem:
    item: Any
    weight: int
