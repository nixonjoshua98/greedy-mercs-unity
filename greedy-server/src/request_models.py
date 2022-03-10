from src.pymodels import BaseModel
from src.static_models.artefacts import ArtefactID


class ArtefactUpgradeModel(BaseModel):
    artefact_id: ArtefactID
    upgrade_levels: int


class LoginData(BaseModel):
    device_id: str


class PrestigeData(BaseModel):
    prestige_stage: int
