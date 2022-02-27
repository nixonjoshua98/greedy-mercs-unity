from __future__ import annotations

from pydantic import Field

from src.pymodels import BaseModel
from src.request import ServerRequest
from typing import Optional


def static_artefacts(request: ServerRequest) -> list[StaticArtefact]:
    d: list[dict] = request.app.get_static_file("artefacts.json")

    return [StaticArtefact.parse_obj(art) for art in d]


class StaticArtefactData:
    def __init__(self, artefacts: list[StaticArtefact]):
        self.artefacts = artefacts

    def get_item(self, iid: int) -> Optional[StaticArtefact]:
        return {art.id: art for art in self.artefacts}.get(iid)


class StaticArtefact(BaseModel):
    id: int = Field(..., alias="artefactId")
    bonus_type: int = Field(..., alias="bonusType")
    max_level: int = Field(1_000, alias="maxLevel")
    cost_expo: float = Field(..., alias="costExpo")
    cost_coeff: float = Field(..., alias="costCoeff")
    base_effect: float = Field(..., alias="baseEffect")
    level_effect: float = Field(..., alias="levelEffect")
