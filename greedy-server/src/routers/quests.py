from fastapi import Depends

from src.auth.handler import (AuthenticatedRequestContext,
                              get_authenticated_context)
from src.common.types import QuestID, QuestType
from src.exceptions import ServerException
from src.handlers.quests import CompleteMercQuestHandler
from src.pymodels import BaseModel
from src.response import ServerResponse
from src.router import APIRouter

router = APIRouter(prefix="/api/quests")


class QuestCompleteModel(BaseModel):
    quest_type: QuestType
    quest_id: QuestID


@router.post("/complete")
async def complete(
    model: QuestCompleteModel,
    ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
    # Quest Handlers #
    _merc_handler: CompleteMercQuestHandler = Depends(),
):
    if model.quest_type == QuestType.MERC_QUEST:
        resp = await _merc_handler.handle(ctx.user_id, ctx.datetime, model.quest_id)

    else:
        raise ServerException(400)

    return ServerResponse(resp)

