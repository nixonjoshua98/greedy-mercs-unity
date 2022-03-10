from fastapi import Depends

from src.auth import AuthenticatedRequestContext, get_authenticated_context
from src.handlers import UpgradeArmouryItemHandler, UpgradeItemResponse
from src.pymodels import BaseModel
from src.response import ServerResponse
from src.router import APIRouter


class ArmouryItemActionModel(BaseModel):
    item_id: int


router = APIRouter()


@router.post("/upgrade")
async def upgrade(
    data: ArmouryItemActionModel,
    user: AuthenticatedRequestContext = Depends(get_authenticated_context),
    handler: UpgradeArmouryItemHandler = Depends(),
):
    resp: UpgradeItemResponse = await handler.handle(user, data.item_id)

    return ServerResponse({
        "item": resp.item,
        "upgradeCost": resp.upgrade_cost,
        "currencyItems": resp.currencies,
    })
