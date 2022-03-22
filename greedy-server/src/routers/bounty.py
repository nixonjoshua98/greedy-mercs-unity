from fastapi import Depends

from src.context import AuthenticatedRequestContext, RequestContext
from src.handlers.auth_handler import get_authenticated_context
from src.handlers.bounties import (BountyClaimResponse, ClaimBountiesHandler,
                                   UpdateBountiesHandler)
from src.response import ServerResponse
from src.router import APIRouter
from src.shared_models import BaseModel

router = APIRouter()


class SetActiveModel(BaseModel):
    bounty_ids: list[int]


@router.get("/claim")
async def claim_points(
    user: AuthenticatedRequestContext = Depends(get_authenticated_context),
    handler: ClaimBountiesHandler = Depends(),
):
    resp: BountyClaimResponse = await handler.handle(user)

    return ServerResponse({
        "claimTime": resp.claim_time,
        "currencyItems": resp.currencies,
        "pointsClaimed": resp.claim_amount,
    })


@router.post("/setactive")
async def set_active_bounties(
    model: SetActiveModel,
    user: AuthenticatedRequestContext = Depends(get_authenticated_context),
    handler: UpdateBountiesHandler = Depends(),
):
    resp = await handler.handle(user, model.bounty_ids)

    return ServerResponse(resp.bounties)
