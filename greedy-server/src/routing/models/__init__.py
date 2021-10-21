
from pydantic import BaseModel


class UserIdentifier(BaseModel):
    device_id: str


class ArmouryItemActionModel(UserIdentifier):
    item_id: int


class ActiveBountyUpdateModel(UserIdentifier):
    bounty_ids: list[int]
