from pydantic import Field

from .basemodels import BaseModel


class PlayerStatsModel(BaseModel):
    total_prestiges: int = Field(0)
    highest_prestige_stage: int = Field(0)
    total_enemies_defeated: int = Field(0)
    total_bosses_defeated: int = Field(0)
    total_taps: int = Field(0)



