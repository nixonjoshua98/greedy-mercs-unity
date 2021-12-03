from fastapi import Depends

from src.authentication.authentication import (AuthenticatedUser,
                                               authenticated_user)
from src.pymodels import BaseModel
from src.routing import APIRouter, ServerResponse
from src.routing.handlers.armoury import (MergeItemHandler, MergeItemResponse,
                                          UpgradeItemHandler,
                                          UpgradeItemResponse)


class ArmouryItemActionModel(BaseModel):
    item_id: int


router = APIRouter(prefix="/api/armoury")


@router.post("/upgrade")
async def upgrade(
    data: ArmouryItemActionModel,
    user: AuthenticatedUser = Depends(authenticated_user),
    handler: UpgradeItemHandler = Depends(),
):
    resp: UpgradeItemResponse = await handler.handle(user, data.item_id)

    return ServerResponse(
        {
            "updatedItem": resp.item.to_client_dict(),
            "currencyItems": resp.currencies.to_client_dict(),
        }
    )


@router.post("/merge")
async def merge(
    data: ArmouryItemActionModel,
    user: AuthenticatedUser = Depends(authenticated_user),
    handler: MergeItemHandler = Depends(),
):
    resp: MergeItemResponse = await handler.handle(user, data.item_id)

    return ServerResponse({"updatedItem": resp.item.to_client_dict()})
