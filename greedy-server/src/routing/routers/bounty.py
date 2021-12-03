from fastapi import Depends

from src.authentication.authentication import (AuthenticatedUser,
                                               authenticated_user)
from src.pymodels import BaseModel
from src.routing import APIRouter, ServerResponse
from src.routing.handlers.bounties import (BountyClaimHandler,
                                           BountyClaimResponse,
                                           UpdateBountiesHandler,
                                           UpdateBountiesResponse)

router = APIRouter(prefix="/api/bounty")


class SetActiveModel(BaseModel):
    bounty_ids: list[int]


@router.get("/claim")
async def claim_points(
        user: AuthenticatedUser = Depends(authenticated_user),
        handler: BountyClaimHandler = Depends()
):
    resp: BountyClaimResponse = await handler.handle(user)

    return ServerResponse(
        {
            "claimTime": resp.claim_time,
            "currencyItems": resp.currencies.to_client_dict(),
            "pointsClaimed": resp.claim_amount,
        }
    )


@router.post("/setactive")
async def set_active_bounties(
    model: SetActiveModel,
    user: AuthenticatedUser = Depends(authenticated_user),
    handler: UpdateBountiesHandler = Depends(),
):
    resp: UpdateBountiesResponse = await handler.handle(user, model.bounty_ids)

    return ServerResponse(resp.bounties.to_client_dict())
