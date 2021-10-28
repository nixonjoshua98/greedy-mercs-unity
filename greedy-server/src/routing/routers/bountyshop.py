from typing import Any

from fastapi import Depends, HTTPException

from src.mongo.repositories.armoury import ArmouryRepository
from src.mongo.repositories.armoury import Fields as ArmouryRepoFields
from src.mongo.repositories.armoury import inject_armoury_repository
from src.mongo.repositories.currency import CurrencyRepository
from src.mongo.repositories.currency import Fields as CurrencyRepoFields
from src.mongo.repositories.currency import inject_currency_repository
from src.pymodels import BaseModel
from src.resources.bountyshop import StaticBountyShopArmouryItem as BSArmouryItem
from src.resources.bountyshop import inject_dynamic_bounty_shop
from src.routing import APIRouter, ServerResponse
from src.routing.common.checks import gt, is_not_none
from src.routing.dependencies.authenticated_user import AuthenticatedUser, inject_user

router = APIRouter(prefix="/api/bountyshop")


# == Request Models == #


class ItemData(BaseModel):
    shop_item: str


@router.post("/purchase")
async def purchase(
    data: ItemData,
    user: AuthenticatedUser = Depends(inject_user),
    # = Database Repositories = #
    currency_repo: CurrencyRepository = Depends(inject_currency_repository),
    armoury_repo: ArmouryRepository = Depends(inject_armoury_repository),
    # = Static/Game Data = #
    bounty_shop=Depends(inject_dynamic_bounty_shop),
):
    item = bounty_shop.get_item(data.shop_item)

    # Verify that the item exists
    is_not_none(item, error="Item was not found")

    item_purchases = 0

    # Check the user still has 'stock' left
    gt(item.purchase_limit, item_purchases, error="Reached purchase limit")

    # Fetch the user currencies
    currencies = await currency_repo.get_user(user.id)

    # Verify that the user can afford to purchase the item
    gt(currencies.bounty_points, item.purchase_cost, error="Cannot afford purchase")

    response_dict = dict()

    # Perform the purchase on ArmouryItems
    if isinstance(item, BSArmouryItem):
        return_dict = await _purchase_armoury_item(user.id, item, repo=armoury_repo)

        response_dict.update(return_dict)

    else:  # Show an error if we got this far
        raise HTTPException(400, detail="Invalid item")

    # Deduct the purchase cost from the user
    currencies = await currency_repo.update_one(
        user.id, {"$inc": {CurrencyRepoFields.BOUNTY_POINTS: -item.purchase_cost}}
    )

    # Return the response. We unpack the reponse_dict here
    return ServerResponse(
        {
            "currencyItems": currencies.response_dict(),
            "purchaseCost": item.purchase_cost,
            **response_dict,
        }
    )


async def _purchase_armoury_item(
    uid, item, *, repo: ArmouryRepository
) -> dict[str, Any]:
    """Process the actual purchase and return a dict to be added to the response."""

    armoury_item = await repo.update_item(
        uid, item.armoury_item, {"$inc": {ArmouryRepoFields.NUM_OWNED: 1}}, upsert=True
    )

    return {"armouryItem": armoury_item.response_dict()}
