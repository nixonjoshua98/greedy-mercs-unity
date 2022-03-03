from src.pymodels import BaseModel


class PrestigeData(BaseModel):
    prestige_stage: int


class LoginModel(BaseModel):
    device_id: str
