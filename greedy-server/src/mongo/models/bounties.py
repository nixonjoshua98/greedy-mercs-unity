from pydantic import Field

import datetime as dt

from .basemodels import BaseDocument, BaseModel


class UserBountyModel(BaseModel):
    bounty_id: int = Field(..., alias="bountyId")
    is_active: bool = Field(default=False, alias="isActive")


class UserBountiesModel(BaseDocument):
    last_claim_time: dt.datetime = Field(..., alias="lastClaimTime")
    bounties: list[UserBountyModel] = Field([])

    @property
    def active_bounties(self) -> list[UserBountyModel]:
        return [b for b in self.bounties if b.is_active]

    def response_dict(self):
        return self.dict(exclude={"id"})  # NB, Field names, instead of aliases
