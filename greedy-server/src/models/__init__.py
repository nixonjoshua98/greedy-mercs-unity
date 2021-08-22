
from pydantic import BaseModel

from typing import Optional


class UserIdentifier(BaseModel):
    device_id: str


class UserLoginDataModel(BaseModel):
    device_id: str


class ArmouryItemActionModel(UserIdentifier):
    item_id: int
