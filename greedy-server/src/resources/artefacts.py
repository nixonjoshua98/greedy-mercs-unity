from __future__ import annotations

from pydantic import Field

from src.utils import load_static_data_file
from src.common.basemodels import BaseModel


def get_static_artefacts() -> list[StaticArtefactModel]:
    d: list[dict] = load_static_data_file("artefacts.json")

    return [StaticArtefactModel.parse_obj(art) for art in d]


class StaticArtefactModel(BaseModel):
    id: int = Field(..., alias="artefactId")

    cost_expo: float = Field(..., alias="costExpo")
    cost_coeff: float = Field(..., alias="costCoeff")
    base_effect: float = Field(..., alias="baseEffect")
    level_effect: float = Field(..., alias="levelEffect")

    bonus_type: int = Field(..., alias="bonusType")

    max_level: int = Field(1_000, alias="maxLevel")
