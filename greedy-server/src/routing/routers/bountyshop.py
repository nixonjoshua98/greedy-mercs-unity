from typing import Any

from fastapi import Depends

from src.mongo.repositories.armoury import ArmouryRepository
from src.mongo.repositories.armoury import Fields as ArmouryRepoFields
from src.mongo.repositories.armoury import inject_armoury_repository
from src.mongo.repositories.currency import CurrencyRepository
from src.mongo.repositories.currency import Fields as CurrencyRepoFields
from src.mongo.repositories.currency import inject_currency_repository
from src.pymodels import BaseModel
from src.resources.bountyshop import StaticBountyShopArmouryItem, inject_dynamic_bounty_shop
from src.routing import APIRouter, ServerResponse
from src.routing.common import checks
from src.authentication.authentication import AuthenticatedUser, inject_authenticated_user

router = APIRouter(prefix="/api/bountyshop")


# == Request Models == #


class ItemData(BaseModel):
    shop_item: str


@router.post("/purchase/armouryitem")
async def purchase_armoury_item(
    data: ItemData,
    user: AuthenticatedUser = Depends(inject_authenticated_user),
    # = Database Repositories = #
    currency_repo: CurrencyRepository = Depends(inject_currency_repository),
    armoury_repo: ArmouryRepository = Depends(inject_armoury_repository),
    # = Static/Game Data = #
    bounty_shop=Depends(inject_dynamic_bounty_shop),
):
    item: StaticBountyShopArmouryItem = bounty_shop.get_item(data.shop_item)

    # Ensure the item is the correct type, otherwise throw an error
    checks.is_instance(item, StaticBountyShopArmouryItem, error="Invalid item")

    item_purchases = 0

    # Check the user still has 'stock' left
    checks.gt(item.purchase_limit, item_purchases, error="Reached purchase limit")

    # Fetch the user currencies
    currencies = await currency_repo.get_user(user.id)

    # Verify that the user can afford to purchase the item
    checks.gte(
        currencies.bounty_points, item.purchase_cost, error="Cannot afford purchase"
    )

    # Deduct the purchase cost from the user
    currencies = await currency_repo.update_one(
        user.id, {"$inc": {CurrencyRepoFields.BOUNTY_POINTS: -item.purchase_cost}}
    )

    # Perform the actual item purchase
    purchase_dict = await _purchase_armoury_item(user.id, item, repo=armoury_repo)

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
