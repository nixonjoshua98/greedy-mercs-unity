from __future__ import annotations

from pydantic import Field

from src.pymodels import BaseModel
from src.routing import ServerRequest


def static_artefacts(request: ServerRequest) -> list[StaticArtefact]:
    d: list[dict] = request.app.get_static_file("artefacts.json")

    return [StaticArtefact.parse_obj(art) for art in d]


class StaticArtefact(BaseModel):
    id: int = Field(..., alias="artefactId")
    bonus_type: int = Field(..., alias="bonusType")
    max_level: int = Field(1_000, alias="maxLevel")
    cost_expo: float = Field(..., alias="costExpo")
    cost_coeff: float = Field(..., alias="costCoeff")
    base_effect: float = Field(..., alias="baseEffect")
    level_effect: float = Field(..., alias="levelEffect")
