from fastapi import Depends

from src.context import AuthenticatedRequestContext, RequestContext
from src.handlers.auth_handler import get_authenticated_context
from src.handlers.quests import (CompleteDailyQuestHandler,
                                 CompleteMercQuestHandler, GetQuestsHandler)
from src.request_models import (CompleteDailyQuestRequestModel,
                                CompleteMercQuestRequestModel)
from src.response import EncryptedServerResponse, ServerResponse
from src.router import APIRouter

router = APIRouter(prefix="/api/quests")


@router.get("")
async def get_quests(
        ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
        handler: GetQuestsHandler = Depends()
):
    resp = await handler.handle(ctx)

    return ServerResponse(resp)


@router.post("/merc")
async def complete_merc_quest(
        model: CompleteMercQuestRequestModel,
        ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
        handler: CompleteMercQuestHandler = Depends(),
):
    resp = await handler.handle(ctx.user_id, model)

    return EncryptedServerResponse(resp)


@router.post("/daily")
async def complete_daily_quest(
        model: CompleteDailyQuestRequestModel,
        ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
        handler: CompleteDailyQuestHandler = Depends(),
):
    resp = await handler.handle(ctx.user_id, model)

    return EncryptedServerResponse(resp)
