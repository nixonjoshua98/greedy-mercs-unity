from pydantic import validator

from src.pymodels import BaseModel


class PrestigeData(BaseModel):
    prestige_stage: int


class LoginModel(BaseModel):
    device_id: str

    @validator("device_id")
    def v_device_id(cls, val):
        return "0"

