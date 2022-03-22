from fastapi import Depends

from src.context import AuthenticatedRequestContext, RequestContext
from src.handlers.artefacts import (BulkUpgradeArtefactsHandler,
                                    BulkUpgradeArtefactsResponse,
                                    UnlockArtefactHandler,
                                    UnlockArtefactResponse)
from src.handlers.auth_handler import get_authenticated_context
from src.request_models import ArtefactUpgradeRequestModel
from src.response import EncryptedServerResponse, ServerResponse
from src.router import APIRouter
from src.shared_models import BaseModel

router = APIRouter(prefix="/api/artefact")


class ArtefactBulkUpgradeModel(BaseModel):
    artefacts: list[ArtefactUpgradeRequestModel]


@router.post("/bulk-upgrade")
async def bulk_upgrade(
    body: ArtefactBulkUpgradeModel,
    ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
    _bulk_upgrade: BulkUpgradeArtefactsHandler = Depends()
):
    response: BulkUpgradeArtefactsResponse = await _bulk_upgrade.handle(ctx.user_id, body.artefacts)

    return EncryptedServerResponse(response)


@router.get("/unlock")
async def unlock(
    user: AuthenticatedRequestContext = Depends(get_authenticated_context),
    handler: UnlockArtefactHandler = Depends(),
):
    resp: UnlockArtefactResponse = await handler.handle(user)

    return ServerResponse({
        "currencyItems": resp.currencies,
        "artefact": resp.artefact,
        "unlockCost": resp.unlock_cost,
    })
