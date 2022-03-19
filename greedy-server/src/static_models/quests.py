
from typing import Optional

from pydantic import Field

from src import utils
from src.common.types import MercID, QuestActionType, QuestID, QuestType
from src.models import BaseModel


class MercQuest(BaseModel):
    quest_id: QuestID = Field(..., alias="questId")
    reward_merc: MercID = Field(..., alias="rewardMercId")


class DailyQuest(BaseModel):
    quest_id: QuestID = Field(..., alias="questId")
    action_type: QuestActionType = Field(..., alias="actionType")
    diamonds_rewarded: int = Field(..., alias="diamondsRewarded")

    num_prestiges: Optional[int] = Field(None, alias="numPrestiges")  # QuestActionType.PRESTIGE


class StaticQuests(BaseModel):
    merc_quests: list[MercQuest] = Field(..., alias="mercQuests")
    daily_quests: list[DailyQuest] = Field(..., alias="dailyQuests")

    def get_quest(self, quest_type: QuestType, quest_id: QuestID):
        if quest_type == QuestType.MERC_QUEST:
            return utils.get(self.merc_quests, quest_id=quest_id)

        elif quest_type == QuestType.DAILY_QUEST:
            return utils.get(self.daily_quests, quest_id=quest_id)

        raise Exception("Invalid quest type discovered")
