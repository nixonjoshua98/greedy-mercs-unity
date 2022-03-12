
from pydantic import Field

from src import utils
from src.common.types import MercID, QuestID, QuestType
from src.pymodels import BaseModel


class MercQuest(BaseModel):
    quest_type = QuestType.MERC_QUEST

    quest_id: QuestID = Field(..., alias="questId")
    reward_merc: MercID = Field(..., alias="rewardMercId")


class StaticQuests(BaseModel):
    merc_quests: list[MercQuest] = Field(..., alias="mercQuests")

    def get_quest(self, quest_type: QuestType, quest_id: QuestID):
        if quest_type == QuestType.MERC_QUEST:
            return utils.get(self.merc_quests, quest_id=quest_id)

        raise Exception("Invalid quest type discovered")
