from __future__ import annotations

from pydantic import Field

from src.pymodels import BaseModel
from src.routing import ServerRequest


def static_armoury(request: ServerRequest) -> list[StaticArmouryItem]:
    d: list[dict] = request.app.get_static_file("armoury.json")

    return [StaticArmouryItem.parse_obj(art) for art in d]


class StaticArmouryItem(BaseModel):
    id: int = Field(..., alias="itemId")
    tier: int = Field(..., alias="itemTier")
    max_merge_lvl: int = Field(..., alias="maxMergeLevel")
    base_merge_cost: int = Field(..., alias="baseMergeCost")
    base_damage_multiplier: float = Field(..., alias="baseDamageMultiplier")
