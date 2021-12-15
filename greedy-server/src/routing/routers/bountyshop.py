from fastapi import Depends

from src.pymodels import BaseModel
from src.request_context import (AuthenticatedRequestContext,
                                 authenticated_context)
from src.routing import APIRouter, ServerResponse

from ..handlers.bountyshop import (PurchaseArmouryItemHandler,
                                   PurchaseArmouryItemResponse)

router = APIRouter()


class ItemData(BaseModel):
    item_id: str


@router.post("/purchase/armouryitem")
async def purchase_armoury_item(
    data: ItemData,
    ctx: AuthenticatedRequestContext = Depends(authenticated_context),
    handler: PurchaseArmouryItemHandler = Depends(PurchaseArmouryItemHandler),
):
    resp: PurchaseArmouryItemResponse = await handler.handle(ctx.user_id, data.item_id)

    return ServerResponse(
        {
            "currencyItems": resp.currencies.client_dict(),
            "purchaseCost": resp.purchase_cost,
            "armouryItem": resp.item.client_dict(),
        }
    )
