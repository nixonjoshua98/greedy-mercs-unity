from fastapi import APIRouter, HTTPException, Depends

from src.common.enums import ItemKey
from src.checks import user_or_raise
from src.routing import ServerRoute, ServerResponse
from src.models import ArmouryItemActionModel

from src.dataloader import DataLoader
from src import resources

from src.common.requestchecks import (
    check_can_afford,
    check_item_is_not_none
)

from src.mongo.repositories.armoury import (
    ArmouryRepository,
    ArmouryItemModel,
    Fields as ArmouryFieldKeys,
    armoury_repository
)

router = APIRouter(prefix="/api/armoury", route_class=ServerRoute)


@router.post("/upgrade")
async def upgrade(
        data: ArmouryItemActionModel,
        armoury_repo: ArmouryRepository = Depends(armoury_repository)
):
    uid = await user_or_raise(data)

    # Verify the item is valid
    check_item_is_valid(data.item_id)

    # Fetch the item data
    user_item = await armoury_repo.get_item(uid, data.item_id)

    # Verify the item exists
    check_item_is_not_none(user_item, error="Attempted to upgrade a locked armoury item")

    # Calculate the upgrade cost for the item
    upgrade_cost = calc_upgrade_cost(user_item)

    # TEMP - Fetch the currency we need
    u_ap = await DataLoader().items.get_item(uid, ItemKey.ARMOURY_POINTS)

    # Verify the user can afford to upgrade the requested item
    check_can_afford(u_ap, upgrade_cost, error="Cannot afford upgrade")

    # Update the request item data
    updated_item = await armoury_repo.update_item(uid, data.item_id, {
        "$inc": {
            ArmouryFieldKeys.LEVEL: 1
        }})

    # TEMP - Deduct the upgrade cost and return all user items AFTER the update
    u_items = await DataLoader().items.update_and_get(uid, {"$inc": {ItemKey.ARMOURY_POINTS: -upgrade_cost}})

    return ServerResponse({"updatedItem": updated_item.response_dict(), "currencyItems": u_items})


@router.post("/evolve")
async def evolve(
        data: ArmouryItemActionModel,
        armoury_repo: ArmouryRepository = Depends(armoury_repository)
):
    uid = await user_or_raise(data)

    # Verify the item is valid
    check_item_is_valid(data.item_id)

    armoury = resources.get_armoury_resources()

    # Fetch the armoury item from the database
    armoury_item = await armoury_repo.get_item(uid, data.item_id)

    # Check that the item exists (is not None)
    check_item_is_not_none(armoury_item, error="User has not unlocked armoury item")

    # Verify the user can evolve the weapon
    check_can_evolve_weapon(armoury_item)

    # Update the item document
    updated_item = await armoury_repo.update_item(uid, data.item_id, {
        "$inc": {
            ArmouryFieldKeys.EVO_LEVEL: 1,
            ArmouryFieldKeys.NUM_OWNED: -armoury.evo_level_cost
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
