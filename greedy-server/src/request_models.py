from src.common.types import QuestID
from src.shared_models import BaseModel, PlayerStatsModel
from src.static_models.artefacts import ArtefactID


class ArtefactUpgradeRequestModel(BaseModel):
    artefact_id: ArtefactID
    upgrade_levels: int


class LoginRequestModel(BaseModel):
    device_id: str


class PrestigeRequestModel(BaseModel):
    prestige_stage: int


class CompleteMercQuestRequestModel(BaseModel):
    quest_id: QuestID
    highest_stage_reached: int


class CompleteDailyQuestRequestModel(BaseModel):
    quest_id: QuestID
    local_daily_stats: PlayerStatsModel
