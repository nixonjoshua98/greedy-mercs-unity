
from pydantic import BaseModel

from typing import Optional


class UserIdentifier(BaseModel):
    """ Authentication Data Model (can be inherited from) """

    device_id: str


class UserLoginDataModel(BaseModel):
    device_id: str
