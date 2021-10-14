from __future__ import annotations

from pydantic import Field

from src.utils import load_static_data_file
from src.common.basemodels import BaseModel


def inject_static_artefacts() -> list[StaticArtefact]:
    """ Inject an instance of the static data """
    d: list[dict] = load_static_data_file("artefacts.json")

    return [StaticArtefact.parse_obj(art) for art in d]


class StaticArtefact(BaseModel):
    id: int = Field(..., alias="artefactId")
    bonus_type: int = Field(..., alias="bonusType")
    max_level: int = Field(1_000, alias="maxLevel")
    cost_expo: float = Field(..., alias="costExpo")
    cost_coeff: float = Field(..., alias="costCoeff")
    base_effect: float = Field(..., alias="baseEffect")
    level_effect: float = Field(..., alias="levelEffect")
