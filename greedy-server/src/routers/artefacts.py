from fastapi import Depends
from pydantic import Field

from src.auth import AuthenticatedRequestContext, get_authenticated_context
from src.handlers import (BulkUpgradeArtefactsHandler,
                          BulkUpgradeArtefactsResponse, UnlockArtefactHandler,
                          UnlockArtefactResponse, UpgradeArtefactHandler,
                          UpgradeArtefactResponse)
from src.pymodels import BaseModel
from src.request_models import ArtefactUpgradeModel
from src.response import ServerResponse
from src.router import APIRouter

router = APIRouter(prefix="/api/artefact")


class ArtefactBulkUpgradeModel(BaseModel):
    artefacts: list[ArtefactUpgradeModel] = Field(..., alias="artefacts")


@router.post("/bulk-upgrade")
async def bulk_upgrade(
    body: ArtefactBulkUpgradeModel,
    ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
    _bulk_upgrade: BulkUpgradeArtefactsHandler = Depends()
):
    response: BulkUpgradeArtefactsResponse = await _bulk_upgrade.handle(ctx.user_id, body.artefacts)

    return ServerResponse(response)


@router.post("/upgrade")
async def upgrade(
    data: ArtefactUpgradeModel,
    user: AuthenticatedRequestContext = Depends(get_authenticated_context),
    handler: UpgradeArtefactHandler = Depends(),
):
    resp: UpgradeArtefactResponse = await handler.handle(user, data.artefact_id, data.upgrade_levels)

    return ServerResponse({
        "currencyItems": resp.currencies,
        "artefact": resp.artefact,
        "upgradeCost": resp.upgrade_cost,
    })


@router.get("/unlock")
async def unlock(
    user: AuthenticatedRequestContext = Depends(get_authenticated_context),
    handler: UnlockArtefactHandler = Depends(),
):
    resp: UnlockArtefactResponse = await handler.handle(user)

    return ServerResponse({
        "currencyItems": resp.currencies.client_dict(),
        "artefact": resp.artefact.client_dict(),
        "unlockCost": resp.unlock_cost,
    })
