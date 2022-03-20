from __future__ import annotations

from typing import NewType

from pydantic import Field

from src.shared_models import BaseModel

ArmouryItemID = NewType("ArmouryItemID", int)


class StaticArmouryItem(BaseModel):
    id: ArmouryItemID = Field(..., alias="itemId")
    bonus_type: int = Field(..., alias="bonusType")
    level_effect: float = Field(..., alias="levelEffect")
    base_effect: float = Field(..., alias="baseEffect")
