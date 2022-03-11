from fastapi import Depends

from src.auth import AuthenticatedRequestContext, get_authenticated_context
from src.handlers import (BountyClaimResponse, ClaimBountiesHandler,
                          UpdateBountiesHandler, UpdateBountiesResponse)
from src.pymodels import BaseModel
from src.response import ServerResponse
from src.router import APIRouter

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
    resp: UpdateBountiesResponse = await handler.handle(user, model.bounty_ids)

    return ServerResponse(resp.bounties)
