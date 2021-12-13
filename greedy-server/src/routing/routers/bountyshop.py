from typing import Any

from fastapi import Depends

from src.authentication import RequestContext, request_context
from src.mongo.repositories.armoury import ArmouryRepository
from src.mongo.repositories.armoury import Fields as ArmouryRepoFields
from src.mongo.repositories.armoury import armoury_repository
from src.mongo.repositories.currency import CurrencyRepository
from src.mongo.repositories.currency import Fields as CurrencyRepoFields
from src.mongo.repositories.currency import currency_repository
from src.pymodels import BaseModel
from src.resources.bountyshop import BountyShopArmouryItem, dynamic_bounty_shop
from src.routing import APIRouter, ServerResponse

router = APIRouter()


class ItemData(BaseModel):
    shop_item: str


@router.post("/purchase/armouryitem")
async def purchase_armoury_item(
    data: ItemData,
    user: RequestContext = Depends(request_context),
    currency_repo: CurrencyRepository = Depends(currency_repository),
    armoury_repo: ArmouryRepository = Depends(armoury_repository),
    bounty_shop=Depends(dynamic_bounty_shop),
):
    item: BountyShopArmouryItem = bounty_shop.get_item(data.shop_item)

    # Ensure the item is the correct type, otherwise throw an error
    if not isinstance(item, BountyShopArmouryItem):
        raise

    item_purchases = 0

    # Check the user still has 'stock' left
    if item_purchases >= item.purchase_limit:
        raise

    # Fetch the user currencies
    currencies = await currency_repo.get_user(user.user_id)

    # Verify that the user can afford to purchase the item
    if item.purchase_cost > currencies.bounty_points:
        raise

    # Deduct the purchase cost from the user
    currencies = await currency_repo.update_one(
        user.user_id, {"$inc": {CurrencyRepoFields.BOUNTY_POINTS: -item.purchase_cost}}
    )

    # Perform the actual item purchase
    purchase_dict = await _purchase_armoury_item(user.user_id, item, repo=armoury_repo)

    # Return the response. We unpack the reponse_dict here
    return ServerResponse(
        {
            "currencyItems": currencies.to_client_dict(),
            "purchaseCost": item.purchase_cost,
            **purchase_dict,
        }
    )


async def _purchase_armoury_item(
    uid, item, *, repo: ArmouryRepository
) -> dict[str, Any]:
    """Process the actual purchase and return a dict to be added to the response."""

    armoury_item = await repo.update_item(
        uid, item.armoury_item, {"$inc": {ArmouryRepoFields.NUM_OWNED: 1}}, upsert=True
    )

    return {"armouryItem": armoury_item.to_client_dict()}
