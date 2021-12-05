from __future__ import annotations

from src import utils
from src.pymodels import BaseModel, Field


def inject_static_bounties() -> StaticBounties:
    d: dict = utils.load_static_data_file("bounties.json")

    return StaticBounties.parse_obj(d)


class StaticBounty(BaseModel):
    id: int = Field(..., alias="bountyId")
    income: int = Field(..., alias="hourlyIncome")
    stage: int = Field(..., alias="unlockStage")


class StaticBounties(BaseModel):
    max_unclaimed_hours: float = Field(..., alias="maxUnclaimedHours")
    max_active_bounties: int = Field(..., alias="maxActiveBounties")

    bounties: list[StaticBounty]
