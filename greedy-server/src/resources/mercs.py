from __future__ import annotations

from src.pymodels import BaseModel, Field
from src.routing import ServerRequest


def inject_merc_data(request: ServerRequest) -> list[StaticMerc]:
    return [StaticMerc.parse_obj(m) for m in request.app.get_static_file("mercs.json")]


class MercPassive(BaseModel):
    type: int = Field(..., alias="bonusType")
    value: float = Field(..., alias="bonusValue")
    unlock_level: int = Field(..., alias="unlockLevel")


class StaticMerc(BaseModel):
    id: int = Field(..., alias="mercId")
    is_default: bool = Field(False, alias="isDefault")
    base_cost: float = Field(..., alias="baseUpgradeCost")
    base_damage: float = Field(..., alias="baseDamage")

    passives: list[MercPassive]
