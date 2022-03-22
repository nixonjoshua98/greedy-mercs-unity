from __future__ import annotations

from src.common.types import BountyID
from src.shared_models import BaseModel, Field


class StaticBounty(BaseModel):
    id: BountyID = Field(..., alias="bountyId")
    income: int = Field(..., alias="hourlyIncome")
    stage: int = Field(..., alias="unlockStage")


class StaticBounties(BaseModel):
    max_unclaimed_hours: float = Field(..., alias="maxUnclaimedHours")
    max_active_bounties: int = Field(..., alias="maxActiveBounties")

    bounties: list[StaticBounty]
