from fastapi import Depends

from src.auth.handler import (AuthenticatedRequestContext,
                              get_authenticated_context)
from src.common.types import QuestID
from src.handlers.quests import CompleteMercQuestHandler
from src.models import BaseModel
from src.response import ServerResponse
from src.router import APIRouter

router = APIRouter(prefix="/api/quests")


class QuestCompleteModel(BaseModel):
    quest_id: QuestID


@router.post("/merc/complete")
async def complete_merc_quest(
    model: QuestCompleteModel,
    ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
    _handler: CompleteMercQuestHandler = Depends(),
):
    resp = await _handler.handle(ctx.user_id, ctx.datetime, model.quest_id)

    return ServerResponse(resp)

