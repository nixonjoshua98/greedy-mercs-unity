from .basemodels import BaseModel


class PlayerStats(BaseModel):
    total_enemies_defeated: int
    total_bosses_defeated: int
    total_taps: int


