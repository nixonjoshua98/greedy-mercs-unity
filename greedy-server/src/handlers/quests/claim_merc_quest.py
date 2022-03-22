from bson import ObjectId
from fastapi import Depends

from src import utils
from src.common.types import MercID
from src.context import AuthenticatedRequestContext, RequestContext
from src.dependencies import get_merc_quests_repo
from src.exceptions import HandlerException
from src.repositories.mercs import (UnlockedMercsRepository,
                                    get_unlocked_mercs_repo)
from src.repositories.quests import MercQuestModel, MercQuestsRepository
from src.request_models import CompleteMercQuestRequestModel
from src.shared_models import BaseModel
from src.static_models.quests import MercQuest

from .create_quests import CreateQuestsHandler, CreateQuestsResponse


class CompleteMercQuestResponse(BaseModel):
    unlocked_merc: MercID


class CompleteMercQuestHandler:
    def __init__(
        self,
        ctx: RequestContext = Depends(),
        quests=Depends(get_merc_quests_repo),
        mercs=Depends(get_unlocked_mercs_repo),
        create_quests: CreateQuestsHandler = Depends(),
    ):
        self.ctx = ctx

        # = Handlers = #
        self._create_quests = create_quests

        # = Repositories = #
        self._quests: MercQuestsRepository = quests
        self._mercs: UnlockedMercsRepository = mercs

    async def handle(self, uid: ObjectId, model: CompleteMercQuestRequestModel) -> CompleteMercQuestResponse:

        # Generate the quests for the user
        user_quests: CreateQuestsResponse = await self._create_quests.handle(uid, self.ctx)

        # Look for the unique quest
        quest_data: MercQuest = utils.get(user_quests.merc_quests, quest_id=model.quest_id)

        # Quest provided is not valid
        if quest_data is None:
            raise HandlerException(500, "Quest not found")

        # Quest has already been completed and rewarded
        elif await self._quests.get_quest(uid, model.quest_id) is not None:
            raise HandlerException(400, "Quest already completed")

        # Quest merc reward is already unlocked
        elif await self._mercs.merc_unlocked(uid, quest_data.reward_merc):
            raise HandlerException(400, "Merc already unlocked")

        # Unlock condition has not been met yet
        elif quest_data.required_stage < quest_data.required_stage:
            raise HandlerException(400, "Unlock conditions not met")

        # Confirm the completion and update the database
        await self.handle_completed_quest(uid, quest_data)

        return CompleteMercQuestResponse(unlocked_merc=quest_data.reward_merc)

    async def handle_completed_quest(self, uid: ObjectId, quest: MercQuest):
        await self._quests.add_quest(MercQuestModel(
            user_id=uid,
            quest_id=quest.quest_id,
            completed_at=self.ctx.datetime
        ))

        await self._mercs.insert_units(uid, [quest.reward_merc])
