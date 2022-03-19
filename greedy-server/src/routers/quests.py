from fastapi import Depends

from src.auth.handler import (AuthenticatedRequestContext,
                              get_authenticated_context)
from src.handlers.quests import (CompleteDailyQuestHandler,
                                 CompleteMercQuestHandler)
from src.request_models import (CompleteDailyQuestRequestModel,
                                CompleteMercQuestRequestModel)
from src.response import EncryptedServerResponse
from src.router import APIRouter

router = APIRouter(prefix="/api/quests")


@router.post("/merc/complete")
async def complete_merc_quest(
    model: CompleteMercQuestRequestModel,
    ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
    _handler: CompleteMercQuestHandler = Depends(),
):
    resp = await _handler.handle(ctx.user_id, ctx.datetime, model)

    return EncryptedServerResponse(resp)


@router.post("/daily/complete")
async def complete_merc_quest(
    model: CompleteDailyQuestRequestModel,
    ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
    _handler: CompleteDailyQuestHandler = Depends(),
):
    resp = await _handler.handle(ctx.user_id, model)

    return EncryptedServerResponse(resp)
