from __future__ import annotations

from pydantic import Field

from src.utils import load_static_data_file
from src.common.basemodels import BaseModel


def inject_static_armoury_data() -> list[StaticArmouryItem]:
    """ Inject an instance of the static data """
    d: list[dict] = load_static_data_file("armoury.json")

    return [StaticArmouryItem.parse_obj(art) for art in d]


class StaticArmouryItem(BaseModel):
    id: int = Field(..., alias="itemId")
    item_tier: int = Field(..., alias="itemTier")
    max_star_level: int = Field(..., alias="maxStarLevel")
    base_star_level_cost: int = Field(..., alias="baseStarLevelCost")
    base_damage_multiplier: float = Field(..., alias="baseDamageMultiplier")