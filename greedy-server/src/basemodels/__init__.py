
from pydantic import BaseModel

from typing import Optional


class UserIdentifier(BaseModel):
    device_id: str
    user_id: Optional[str] = None


class UserLoginModel(BaseModel):
    device_id: str


class ItemPurchaseModel(UserIdentifier):
    item_id: int
