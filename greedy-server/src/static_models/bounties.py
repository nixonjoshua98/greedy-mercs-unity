from __future__ import annotations

from typing import NewType

from src.pymodels import BaseModel, Field

BountyID = NewType("BountyID", int)


class StaticBounty(BaseModel):
    id: BountyID = Field(..., alias="bountyId")
    income: int = Field(..., alias="hourlyIncome")
    stage: int = Field(..., alias="unlockStage")


class StaticBounties(BaseModel):
    max_unclaimed_hours: float = Field(..., alias="maxUnclaimedHours")
    max_active_bounties: int = Field(..., alias="maxActiveBounties")

    bounties: list[StaticBounty]
