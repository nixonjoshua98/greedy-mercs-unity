
from typing import Optional

from pydantic import Field

from src.common.types import MercID, QuestActionType, QuestID
from src.shared_models import BaseModel


class MercQuest(BaseModel):
    quest_id: QuestID = Field(..., alias="questId")
    reward_merc: MercID = Field(..., alias="rewardMercId")
    required_stage: int = Field(..., alias="requiredStage")


class DailyQuest(BaseModel):
    quest_id: QuestID = Field(..., alias="questId")
    action_type: QuestActionType = Field(..., alias="actionType")
    diamonds_rewarded: int = Field(..., alias="diamondsRewarded")

    num_prestiges: Optional[int] = Field(None, alias="numPrestiges")  # QuestActionType.PRESTIGE


class StaticQuests(BaseModel):
    merc_quests: list[MercQuest] = Field(..., alias="mercQuests")
    daily_quests: list[DailyQuest] = Field(..., alias="dailyQuests")