import datetime as dt

from bson import ObjectId
from fastapi import Depends

from src.common.types import MercID, QuestType
from src.dependencies import get_merc_quests_repo, get_static_quests
from src.exceptions import HandlerException
from src.models import BaseModel
from src.mongo.mercs import UnlockedMercsRepository, get_unlocked_mercs_repo
from src.mongo.quests import MercQuestModel, MercQuestsRepository
from src.request_models import CompleteMercQuestRequestModel
from src.static_models.quests import MercQuest, StaticQuests


class GetUserQuestsResponse(BaseModel):
    ...


class GetUserQuestsHandler:
    def __init__(self):
        ...

    async def handle(self):
        return GetUserQuestsResponse()