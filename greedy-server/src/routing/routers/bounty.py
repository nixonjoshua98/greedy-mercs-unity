from fastapi import Depends

from src.pymodels import BaseModel
from src.request_context import (AuthenticatedRequestContext,
                                 authenticated_context)
from src.routing import APIRouter, ServerResponse
from src.routing.handlers.bounties import (BountyClaimHandler,
                                           BountyClaimResponse,
                                           UpdateBountiesHandler,
                                           UpdateBountiesResponse)

router = APIRouter()


class SetActiveModel(BaseModel):
    bounty_ids: list[int]


@router.get("/claim")
async def claim_points(
    user: AuthenticatedRequestContext = Depends(authenticated_context),
    handler: BountyClaimHandler = Depends(),
):
    resp: BountyClaimResponse = await handler.handle(user)

    return ServerResponse(
        {
            "claimTime": resp.claim_time,
            "currencyItems": resp.currencies.client_dict(),
            "pointsClaimed": resp.claim_amount,
        }
    )


@router.post("/setactive")
async def set_active_bounties(
    model: SetActiveModel,
    user: AuthenticatedRequestContext = Depends(authenticated_context),
    handler: UpdateBountiesHandler = Depends(),
):
    resp: UpdateBountiesResponse = await handler.handle(user, model.bounty_ids)

    return ServerResponse(resp.bounties.client_dict())
