from __future__ import annotations

from src.common.enums import AttackType
from src.pymodels import BaseModel, Field
from src.request import ServerRequest


def inject_merc_data(request: ServerRequest) -> StaticMercsData:
    return StaticMercsData.parse_obj(request.app.get_static_file("mercs.json5"))


class PassiveBonus(BaseModel):
    type: int = Field(..., alias="bonusType")
    value: float = Field(..., alias="bonusValue")


class MercPassive(BaseModel):
    unlock_level: int = Field(..., alias="unlockLevel")


class StaticMerc(BaseModel):
    id: int = Field(..., alias="mercId")
    attack_type: AttackType = Field(AttackType.MELEE, alias="attackType")
    name: str = Field("Missing Merc name", alias="name")
    is_default: bool = Field(False, alias="isDefault")
    base_cost: float = Field(..., alias="baseUpgradeCost")
    base_damage: float = Field(..., alias="baseDamage")

    passives: list[MercPassive]


class StaticMercsData(BaseModel):
    passives: list[PassiveBonus] = Field(..., alias="passives")
    mercs: list[StaticMerc] = Field(..., alias="mercs")
