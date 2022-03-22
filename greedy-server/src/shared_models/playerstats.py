from pydantic import Field

from .basemodels import BaseModel


class PlayerStatsFields:
    total_enemies_defeated = "totalEnemiesDefeated"
    total_bosses_defeated = "totalBossesDefeated"
    total_taps = "totalTaps"


class PlayerStatsModel(BaseModel):
    total_prestiges: int = Field(0)
    highest_prestige_stage: int = Field(0)
    total_enemies_defeated: int = Field(0, alias=PlayerStatsFields.total_enemies_defeated)
    total_bosses_defeated: int = Field(0, alias=PlayerStatsFields.total_bosses_defeated)
    total_taps: int = Field(0, alias=PlayerStatsFields.total_taps)
