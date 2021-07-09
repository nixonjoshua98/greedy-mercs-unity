
from pydantic import BaseModel


class UserLoginModel(BaseModel):
    device_id: str
