from fastapi import Depends

from src.authentication import RequestContext, request_context
from src.pymodels import BaseModel
from src.routing import APIRouter, ServerResponse

from ..handlers.bountyshop import (PurchaseArmouryItemHandler,
                                   PurchaseArmouryItemResponse)

router = APIRouter()


class ItemData(BaseModel):
    item_id: str


@router.post("/purchase/armouryitem")
async def purchase_armoury_item(
    data: ItemData,
    ctx: RequestContext = Depends(request_context),
    handler: PurchaseArmouryItemHandler = Depends(PurchaseArmouryItemHandler)
):
    resp: PurchaseArmouryItemResponse = await handler.handle(ctx.user_id, data.item_id, ctx.prev_daily_reset)

    return ServerResponse({
        "currencyItems": resp.currencies.to_client_dict(),
        "purchaseCost": resp.purchase_cost,
        "armouryItem": resp.item.to_client_dict(),
    })
