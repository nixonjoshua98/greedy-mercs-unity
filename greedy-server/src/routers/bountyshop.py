from fastapi import Depends

from src.context import AuthenticatedRequestContext, RequestContext
from src.handlers import (PurchaseArmouryItemHandler,
                          PurchaseArmouryItemResponse, PurchaseCurrencyHandler,
                          PurchaseCurrencyResponse)
from src.handlers.auth_handler import get_authenticated_context
from src.response import ServerResponse
from src.router import APIRouter
from src.shared_models import BaseModel

router = APIRouter()


class ItemData(BaseModel):
    item_id: str


@router.post("/purchase/armouryitem")
async def purchase_armoury_item(
    data: ItemData,
    ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
    handler: PurchaseArmouryItemHandler = Depends(),
):
    resp: PurchaseArmouryItemResponse = await handler.handle(ctx.user_id, data.item_id)

    return ServerResponse({
        "currencyItems": resp.currencies,
        "purchaseCost": resp.purchase_cost,
        "armouryItem": resp.item,
    })


@router.post("/purchase/currency")
async def purchase_armoury_item(
    data: ItemData,
    ctx: AuthenticatedRequestContext = Depends(get_authenticated_context),
    handler: PurchaseCurrencyHandler = Depends(),
):
    resp: PurchaseCurrencyResponse = await handler.handle(ctx.user_id, data.item_id)

    return ServerResponse({
        "currencyItems": resp.currencies,
        "purchaseCost": resp.purchase_cost,
        "currencyGained": resp.currency_gained
    })
