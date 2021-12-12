from fastapi import Depends

from src.authentication.authentication import (AuthenticatedUser,
                                               authenticated_user)
from src.pymodels import BaseModel
from src.routing import APIRouter, ServerResponse

from ..handlers.artefacts import (UnlockArtefactHandler,
                                  UnlockArtefactResponse,
                                  UpgradeArtefactHandler,
                                  UpgradeArtefactResponse)

router = APIRouter()


class ArtefactUpgradeModel(BaseModel):
    artefact_id: int
    upgrade_levels: int


@router.post("/upgrade")
async def upgrade(
    data: ArtefactUpgradeModel,
    user: AuthenticatedUser = Depends(authenticated_user),
    handler: UpgradeArtefactHandler = Depends(UpgradeArtefactHandler)
):
    resp: UpgradeArtefactResponse = await handler.handle(user, data.artefact_id, data.upgrade_levels)

    return ServerResponse({
        "currencyItems": resp.currencies.to_client_dict(),
        "artefact": resp.artefact.to_client_dict(),
        "upgradeCost": resp.upgrade_cost,
    })


@router.get("/unlock")
async def unlock(
    user: AuthenticatedUser = Depends(authenticated_user),
    handler: UnlockArtefactHandler = Depends(),
):
    resp: UnlockArtefactResponse = await handler.handle(user)

    return ServerResponse({
        "currencyItems": resp.currencies.to_client_dict(),
        "artefact": resp.artefact.to_client_dict(),
        "unlockCost": resp.unlock_cost,
    })
