from fastapi import Depends

from src.pymodels import BaseModel
from src.request_context import (AuthenticatedRequestContext,
                                 inject_authenticated_context)
from src.routing import APIRouter, ServerResponse
from src.routing.handlers.armoury import (UpgradeItemHandler,
                                          UpgradeItemResponse)


class ArmouryItemActionModel(BaseModel):
    item_id: int


router = APIRouter()


@router.post("/upgrade")
async def upgrade(
    data: ArmouryItemActionModel,
    user: AuthenticatedRequestContext = Depends(inject_authenticated_context),
    handler: UpgradeItemHandler = Depends(),
):
    resp: UpgradeItemResponse = await handler.handle(user, data.item_id)

    return ServerResponse(
        {
            "item": resp.item.client_dict(),
            "upgradeCost": resp.upgrade_cost,
            "currencyItems": resp.currencies.client_dict(),
        }
    )
