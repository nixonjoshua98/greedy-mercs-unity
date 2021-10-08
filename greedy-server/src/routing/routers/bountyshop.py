from typing import Any

from fastapi import HTTPException, Depends

from src.checks import user_or_raise
from src.models import UserIdentifier

from src.dataloader import DataLoader

from src.routing import ServerResponse, APIRouter
from src.routing.common.checks import check_is_not_none, check_greater_than

from src.resources.bountyshop import BountyShop, ArmouryItem as BSArmouryItem

from src.mongo.repositories.currencies import CurrenciesRepository, Fields as CurrencyRepoFields, currencies_repository
from src.mongo.repositories.armoury import ArmouryRepository, Fields as ArmouryRepoFields, armoury_repository

router = APIRouter(prefix="/api/bountyshop")


# == Request Models == #

class ItemData(UserIdentifier):
    shop_item: str


@router.post("/purchase")
async def purchase(
        data: ItemData,
        currency_repo: CurrenciesRepository = Depends(currencies_repository),
        armoury_repo: ArmouryRepository = Depends(armoury_repository)
):
    uid = await user_or_raise(data)

    shop = BountyShop()

    item = shop.get_item(data.shop_item)

    # Verify that the item exists
    check_is_not_none(item, error="Item was not found")

    item_purchases = await DataLoader().bounty_shop.get_daily_purchases(uid, item.id)

    # Check the user still has 'stock' left
    check_greater_than(item.purchase_limit, item_purchases, error="Reached purchase limit")

    # Fetch the user currencies
    currencies = await currency_repo.get_user(uid)

    # Verify that the user can afford to purchase the item
    check_greater_than(currencies.bounty_points, item.purchase_cost, error="Cannot afford purchase")

    response_dict = dict()

    # Perform the purchase on 'ArmouryItem's
    if isinstance(item, BSArmouryItem):
        response_dict.update(await _purchase_armoury_item(uid, item, repo=armoury_repo))

    else:  # Show an error if we got this far
        raise HTTPException(400, detail="Invalid item")

    # Deduct the purchase cost from the user
    currencies = await currency_repo.update_one(uid, {
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
