from fastapi import HTTPException, Depends

from src.checks import user_or_raise
from src.routing import ServerResponse, APIRouter
from src.models import ArmouryItemActionModel

from src import resources

from src.routing.common.checks import (
    check_greater_than,
    check_is_not_none
)

from src.mongo.repositories.armoury import (
    ArmouryRepository,
    ArmouryItemModel,
    Fields as ArmouryFieldKeys,
    armoury_repository
)

from src.mongo.repositories.currencies import (
    CurrenciesRepository,
    Fields as CurrencyRepoFields,
    currencies_repository
)

router = APIRouter(prefix="/api/armoury")


@router.post("/upgrade")
async def upgrade(
        data: ArmouryItemActionModel,
        armoury_repo: ArmouryRepository = Depends(armoury_repository),
        currency_repo: CurrenciesRepository = Depends(currencies_repository)
):
    uid = await user_or_raise(data)

    # Verify the item is valid
    check_item_is_valid(data.item_id)

    # Fetch the item data
    user_item = await armoury_repo.get_item(uid, data.item_id)

    # Verify the item exists
    check_is_not_none(user_item, error="Attempted to upgrade a locked armoury item")

    # Calculate the upgrade cost for the item
    upgrade_cost = calc_upgrade_cost(user_item)

    # Fetch the currency we need
    currencies = await currency_repo.get_user(uid)

    # Verify the user can afford to upgrade the requested item
    check_greater_than(currencies.armoury_points, upgrade_cost, error="Cannot afford upgrade")

    # Deduct the upgrade cost and return all user items AFTER the update
    currencies = await currency_repo.update_one(uid, {
        "$inc": {
            CurrencyRepoFields.ARMOURY_POINTS: -upgrade_cost
        }
    })

    # Update the request item data
    updated_item = await armoury_repo.update_item(uid, data.item_id, {
        "$inc": {
            ArmouryFieldKeys.LEVEL: 1
        }})

    return ServerResponse({"updatedItem": updated_item.response_dict(), "currencyItems": currencies.response_dict()})


@router.post("/evolve")
async def evolve(
        data: ArmouryItemActionModel,
        armoury_repo: ArmouryRepository = Depends(armoury_repository)
):
    uid = await user_or_raise(data)

    # Verify the item is valid
    check_item_is_valid(data.item_id)

    # Fetch the armoury item from the database
    armoury_item = await armoury_repo.get_item(uid, data.item_id)

    # Check that the item exists (is not None)
    check_is_not_none(armoury_item, error="User has not unlocked armoury item")

    # Verify the user can evolve the weapon
    check_can_evolve_weapon(armoury_item)

    # Update the item document
    updated_item = await armoury_repo.update_item(uid, data.item_id, {
        "$inc": {
            ArmouryFieldKeys.EVO_LEVEL: 1,
            ArmouryFieldKeys.NUM_OWNED: -resources.get_armoury_resources().evo_level_cost
        }})

    return ServerResponse({"updateditem": updated_item.response_dict()})


# == Calculations == #

def calc_upgrade_cost(item: ArmouryItemModel):
    armoury = resources.get_armoury_resources()

    static_item = armoury.items[item.item_id]

    return 5 + (static_item.tier + 1) + item.level


# == Checks == #

def check_can_evolve_weapon(item: ArmouryItemModel):
    armoury = resources.get_armoury_resources()

    is_max_level = item.evo_level < armoury.max_evo_level
    can_evolve = item.owned >= (armoury.evo_level_cost + 1)

    if is_max_level or not can_evolve:
        raise HTTPException(400, detail="Cannot evolve item")

    return True


def check_item_is_valid(artefact_id):
    s_items = resources.get_armoury_resources()

    if artefact_id not in s_items.items.keys():
        raise HTTPException(400, detail="Armoury item is not valid")
