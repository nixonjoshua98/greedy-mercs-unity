from src.pymodels import BaseModel
from pydantic import validator


class PrestigeData(BaseModel):
    prestige_stage: int


class LoginModel(BaseModel):
    device_id: str

    @validator("device_id")
    def xx(cls, val):
        return "123"
