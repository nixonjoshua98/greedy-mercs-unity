from __future__ import annotations

from src.pymodels import BaseModel, Field
from src.routing import ServerRequest


def inject_static_bounties(request: ServerRequest) -> StaticBounties:
    d: dict = request.app.get_static_file("bounties.json")

    return StaticBounties.parse_obj(d)


class StaticBounty(BaseModel):
    id: int = Field(..., alias="bountyId")
    income: int = Field(..., alias="hourlyIncome")
    stage: int = Field(..., alias="unlockStage")


class StaticBounties(BaseModel):
    max_unclaimed_hours: float = Field(..., alias="maxUnclaimedHours")
    max_active_bounties: int = Field(..., alias="maxActiveBounties")

    bounties: list[StaticBounty]
