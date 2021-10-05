import math
import random

from fastapi import APIRouter, HTTPException, Depends

from src.resources.artefacts import ArtefactResourceData
from src.checks import user_or_raise
from src.common import formulas
from src.common.enums import ItemKey
from src.routing import ServerRoute, ServerResponse
from src.models import UserIdentifier
from src.dataloader import DataLoader
from src import resources

from src.mongo.repositories.artefacts import (
    ArtefactsRepository,
    ArtefactModel,
    Fields as ArtefactsRepoFields,
    artefacts_repository
)

router = APIRouter(prefix="/api/artefact", route_class=ServerRoute)


# == Models == #

class ArtefactUpgradeModel(UserIdentifier):
    artefact_id: int
    upgrade_levels: int


# == Endpoints == #

@router.post("/upgrade")
async def upgrade(
        data: ArtefactUpgradeModel,
        artefacts_repo: ArtefactsRepository = Depends(artefacts_repository)
):
    uid = await user_or_raise(data)

    # Load the related artefact
    user_art = await artefacts_repo.get_one_artefact(uid, data.artefact_id)

    # Verify that the user has the artefact unlocked
    check_artefact_exists(user_art)

    # Check that upgrading this artefact will not exceed the max level
    check_artefact_within_max_level(user_art, data.upgrade_levels)

    # Calculate the upgrade cost for the artefact
    upgrade_cost = calc_upgrade_cost(user_art, data.upgrade_levels)

    # TEMP - Fetch the currency to upgrade the item
    u_pp = await DataLoader().items.get_item(uid, ItemKey.PRESTIGE_POINTS)

    # Check that the user can afford the upgrade cost
    check_has_enough_currency(u_pp, upgrade_cost, error="Cannot afford to upgrade artefact")

    u_items = await DataLoader().items.update_and_get(uid, {"$inc": {ItemKey.PRESTIGE_POINTS: -upgrade_cost}})

    # Update the artefact
    await artefacts_repo.update_one_artefact(uid, data.artefact_id, {
        "$inc": {
            ArtefactsRepoFields.LEVEL: data.upgrade_levels
        }
    })

    all_artefacts = await artefacts_repo.get_all_artefacts(uid)

    return ServerResponse({"userCurrencies": u_items, "userArtefacts": [art.response_dict() for art in all_artefacts]})


@router.post("/unlock")
async def unlock(
        data: UserIdentifier,
        artefacts_repo: ArtefactsRepository = Depends(artefacts_repository)
):
    uid = await user_or_raise(data)

    # Fetch all user artefacts
    u_artefacts = await artefacts_repo.get_all_artefacts(uid)

    u_pp = await DataLoader().items.get_item(uid, ItemKey.PRESTIGE_POINTS)

    # Verify that the user still has an artefact available to unlock
    check_not_unlocked_all_artefacts(u_artefacts)

    # Calculate the artefact cost
    unlock_cost = calc_unlock_cost(u_artefacts)

    # Verify that the user can afford the unlock cost
    check_has_enough_currency(u_pp, unlock_cost, error="Cannot afford unlock cost")

    # Get the new artefact id
    new_art_id = get_new_artefact(u_artefacts)

    # Add the new artefact
    await artefacts_repo.add_new_artefact(uid, new_art_id)

    # Update the purchase currency
    u_items = await DataLoader().items.update_and_get(uid, {
        "$inc": {
            ItemKey.PRESTIGE_POINTS: -unlock_cost
        }
    })

    # Fetch all user artefacts
    all_arts = await artefacts_repo.get_all_artefacts(uid)

    return ServerResponse({
        "newArtefactId": new_art_id,
        "userCurrencies": u_items,
        "userArtefacts": [art.response_dict() for art in all_arts],
    })


# == Calculations == #

def calc_unlock_cost(artefacts: list[ArtefactModel]):
    return math.floor(max(1, num_arts := len(artefacts) - 2) * math.pow(1.35, num_arts))


def get_new_artefact(artefacts: list[ArtefactModel]):
    s_arts = resources.get_artefacts_data().artefacts

    return random.choice(list(set(list(s_arts.keys())) - set(list([art.id for art in artefacts]))))


def calc_upgrade_cost(artefact: ArtefactModel, levels: int) -> int:
    static_art = resources.get_artefacts_data().artefacts[artefact.artefact_id]

    return formulas.upgrade_artefact_cost(static_art.cost_coeff, static_art.cost_expo, artefact.level, levels)


# == Checks == #

def check_not_unlocked_all_artefacts(artefacts: list[ArtefactModel]):
    static_artefacts = resources.get_artefacts_data().artefacts

    if len(artefacts) >= len(static_artefacts):
        raise HTTPException(400, detail="Max number of artefacts unlocked")


def check_has_enough_currency(currency, value, *, error: str):
    if value > currency:
        raise HTTPException(400, detail=error)


def check_artefact_exists(artefact: ArtefactModel):
    if artefact is None:
        raise HTTPException(400, detail="Artefact is not unlocked")


def check_artefact_within_max_level(artefact: ArtefactModel, levels: int):
    static_artefact: ArtefactResourceData = resources.get_artefacts_data().artefacts[artefact.artefact_id]

    if (artefact.level + levels) > static_artefact.max_level:
        raise HTTPException(400, detail="Level will exceed max level")
