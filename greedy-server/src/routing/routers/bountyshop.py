from typing import Any

from fastapi import HTTPException, Depends

from src.pymodels import BaseModel
from src.routing import ServerResponse, APIRouter
from src.routing.common.checks import check_is_not_none, check_greater_than
from src.routing.dependencies.authenticated_user import AuthenticatedUser, inject_user

from src.resources.bountyshop import StaticBountyShopArmouryItem as BSArmouryItem, inject_dynamic_bounty_shop

from src.mongo.repositories.currencies import (
    CurrenciesRepository,
    Fields as CurrencyRepoFields,
    inject_currencies_repository
)
from src.mongo.repositories.armoury import ArmouryRepository, Fields as ArmouryRepoFields, inject_armoury_repository

router = APIRouter(prefix="/api/bountyshop")


# == Request Models == #

class ItemData(BaseModel):
    shop_item: str


@router.post("/purchase")
async def purchase(
        data: ItemData,
        user: AuthenticatedUser = Depends(inject_user),

        # = Database Repositories = #
        currency_repo: CurrenciesRepository = Depends(inject_currencies_repository),
        armoury_repo: ArmouryRepository = Depends(inject_armoury_repository),

        # = Static/Game Data = #
        bounty_shop=Depends(inject_dynamic_bounty_shop)
):
    item = bounty_shop.get_item(data.shop_item)

    # Verify that the item exists
    check_is_not_none(item, error="Item was not found")

    item_purchases = 0

    # Check the user still has 'stock' left
    check_greater_than(item.purchase_limit, item_purchases, error="Reached purchase limit")

    # Fetch the user currencies
    currencies = await currency_repo.get_user(user.id)

    # Verify that the user can afford to purchase the item
    check_greater_than(currencies.bounty_points, item.purchase_cost, error="Cannot afford purchase")

    response_dict = dict()

    # Perform the purchase on ArmouryItems
    if isinstance(item, BSArmouryItem):
        return_dict = await _purchase_armoury_item(user.id, item, repo=armoury_repo)

        response_dict.update(return_dict)

    else:  # Show an error if we got this far
        raise HTTPException(400, detail="Invalid item")

    # Deduct the purchase cost from the user
    currencies = await currency_repo.update_one(user.id, {
        "$inc": {
            CurrencyRepoFields.BOUNTY_POINTS: -item.purchase_cost
        }
    })

    # Return the response. We unpack the reponse_dict here
    return ServerResponse({"currencyItems": currencies.response_dict(), **response_dict})


async def _purchase_armoury_item(uid, item, *, repo: ArmouryRepository) -> dict[str, Any]:
    """ Process the actual purchase and return a dict to be added to the response. """

    armoury_item = await repo.update_item(uid, item.armoury_item, {
        "$inc": {
            ArmouryRepoFields.NUM_OWNED: 1
        }
    }, upsert=True)

    return {"armouryItem": armoury_item.response_dict()}
