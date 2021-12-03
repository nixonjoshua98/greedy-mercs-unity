from __future__ import annotations

from pydantic import Field

from src.pymodels import BaseModel
from src.utils import load_static_data_file


def static_armoury() -> list[StaticArmouryItem]:
    d: list[dict] = load_static_data_file("armoury.json")

    return [StaticArmouryItem.parse_obj(art) for art in d]


class StaticArmouryItem(BaseModel):
    id: int = Field(..., alias="itemId")
    tier: int = Field(..., alias="itemTier")
    max_merge_lvl: int = Field(..., alias="maxMergeLevel")
    base_merge_cost: int = Field(..., alias="baseMergeCost")
    base_damage_multiplier: float = Field(..., alias="baseDamageMultiplier")
