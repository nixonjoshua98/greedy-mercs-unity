from fastapi import Depends

from src import utils
from src.pymodels import BaseModel
from src.routing import ServerResponse, APIRouter
from src.routing.dependencies.authenticated_user import AuthenticatedUser, inject_user
from src.resources.armoury import inject_static_armoury, StaticArmouryItem
from src.routing.common.checks import check_greater_than, check_is_not_none

from src.mongo.repositories.armoury import (
    ArmouryRepository,
    ArmouryItemModel,
    Fields as ArmouryFieldKeys,
    inject_armoury_repository
)

from src.mongo.repositories.currencies import (
    CurrenciesRepository,
    Fields as CurrencyRepoFields,
    inject_currencies_repository
)


class ArmouryItemActionModel(BaseModel):
    item_id: int


router = APIRouter(prefix="/api/armoury")


@router.post("/upgrade")
async def upgrade(
        data: ArmouryItemActionModel,
        user: AuthenticatedUser = Depends(inject_user),

        # = Static Data = #
        s_armoury_items: list[StaticArmouryItem] = Depends(inject_static_armoury),

        # = Database Repositories = #
        armoury_repo: ArmouryRepository = Depends(inject_armoury_repository),
        currency_repo: CurrenciesRepository = Depends(inject_currencies_repository)
):
    s_item = utils.get(s_armoury_items, id=data.item_id)

    # Verify the item is valid
    check_is_not_none(s_item, error="Invalid item")

    # Fetch the item data
    u_item = await armoury_repo.get_one_item(user.id, data.item_id)

    # Verify the item exists
    check_is_not_none(u_item, error="Attempted to upgrade a locked armoury item")

    # Calculate the upgrade cost for the item
    upgrade_cost = calc_upgrade_cost(s_item, u_item)

    # Fetch the currency we need
    currencies = await currency_repo.get_user(user.id)

    # Verify the user can afford to upgrade the requested item
    check_greater_than(currencies.armoury_points, upgrade_cost, error="Cannot afford upgrade")

    # Deduct the upgrade cost and return all user items AFTER the update
    currencies = await currency_repo.update_one(user.id, {
        "$inc": {
            CurrencyRepoFields.ARMOURY_POINTS: -upgrade_cost
        }
    })

    # Update the requested item data
    updated_item = await armoury_repo.update_item(user.id, data.item_id, {
        "$inc": {
            ArmouryFieldKeys.LEVEL: 1
        }}, upsert=False)

    return ServerResponse({"updatedItem": updated_item.response_dict(), "currencyItems": currencies.response_dict()})


@router.post("/merge")
async def merge(
        data: ArmouryItemActionModel,
        user: AuthenticatedUser = Depends(inject_user),
        # = Static Data = #
        s_armoury_items: list[StaticArmouryItem] = Depends(inject_static_armoury),
        # = Database Repositories = #
        armoury_repo: ArmouryRepository = Depends(inject_armoury_repository)
):
    # Pull the static item from the list
    s_item = utils.get(s_armoury_items, id=data.item_id)

    # Verify the item is valid
    check_is_not_none(s_item, error="Invalid item")

    # Fetch the armoury item from the database
    u_item = await armoury_repo.get_one_item(user.id, data.item_id)

    # Check that the item exists
    check_is_not_none(u_item, error="User has not unlocked armoury item")

    # Check that the user will not exceed the max level
    check_greater_than(s_item.max_merge_lvl, u_item.merge_lvl, error="Item is at max merge level")

    # Calculate the upgrade cost (This is the # of owned item required)
    merge_cost: int = calc_merge_cost(s_item, u_item)

    # Verify that the user has enough owned items to do the upgrade
    check_greater_than(u_item.owned, merge_cost, error="Cannot afford upgrade")

    # Update the item document
    updated_item = await armoury_repo.update_item(user.id, data.item_id, {
        "$inc": {
            ArmouryFieldKeys.MERGE_LEVEL: 1,
            ArmouryFieldKeys.NUM_OWNED: -merge_cost
        }}, upsert=False)

    return ServerResponse({"updateditem": updated_item.response_dict()})


# == Calculations == #

def calc_upgrade_cost(s_item: StaticArmouryItem, item: ArmouryItemModel) -> int:
    return 5 + (s_item.item_tier + 1) + item.level


def calc_merge_cost(s_item: StaticArmouryItem, _: ArmouryItemModel) -> int:
    return s_item.base_merge_cost
