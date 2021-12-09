from __future__ import annotations
from src.routing import ServerRequest
from src.pymodels import Field, BaseModel


def inject_merc_data(request: ServerRequest) -> list[StaticMerc]:
    return [StaticMerc.parse_obj(m) for m in request.app.get_static_file("mercs.json")]


class MercPassive(BaseModel):
    type: int = Field(..., alias="bonusType")
    value: float = Field(..., alias="bonusValue")
    unlock_level: int = Field(..., alias="unlockLevel")


class StaticMerc(BaseModel):
    id: int = Field(..., alias="mercId")
    unlock_cost: float = Field(..., alias="unlockCost")
    base_damage: float = Field(..., alias="baseDamage")

    passives: list[MercPassive]
