import random

from fastapi import APIRouter, HTTPException

from src import resources
from src.resources import ArtefactData
from src.svrdata import Artefacts
from src.checks import user_or_raise
from src.common import formulas
from src.routing import CustomRoute, ServerResponse
from src.models import UserIdentifier

from src.database import mongo, ItemKeys

router = APIRouter(prefix="/api/artefact", route_class=CustomRoute)


# Models
class ArtefactUpgradeModel(UserIdentifier):
    artefact_id: int
    purchase_levels: int


@router.post("/upgrade")
def upgrade(data: ArtefactUpgradeModel):
    uid = user_or_raise(data)

    # Load the artefact resource
    artefact: ArtefactData = resources.get_artefacts().artefacts[data.artefact_id]

    # Pull the data and raise an error if it does not exist
    if (art := Artefacts.find_one(uid, data.artefact_id)) is None:
        raise HTTPException(400, {"error": "Artefact is not unlocked"})

    # Upgrading will exceed the max level
    elif (art["level"] + data.purchase_levels) > artefact.max_level:
        raise HTTPException(400, {"error": "Level will exceed max level"})

    # Calculate the upgrade cost, and pull the currency from the database
    cost = _artefact_upgrade_cost(artefact, art["level"], data.purchase_levels)
    points = mongo.items.get_item(uid, ItemKeys.PRESTIGE_POINTS)

    # Perform the upgrade check (and calculate the upgrade cost)
    if cost > points:  # Raise a HTTP error so the request is aborted
        raise HTTPException(400, {"error": "Cannot afford upgrade cost"})

    # Update the artefact (and pull the new artefact data)
    items = mongo.items.update_and_find(uid, {"$inc": {ItemKeys.PRESTIGE_POINTS: -cost}})

    Artefacts.update_one(uid, data.artefact_id, {"$inc": {"level": data.purchase_levels}})

    return ServerResponse({"userItems": items, "userArtefacts": Artefacts.find(uid)})


@router.post("/unlock")
def unlock(data: UserIdentifier):
    uid = user_or_raise(data)

    artefacts = resources.get_artefacts().artefacts

    # Pull user data from the database
    points = mongo.items.get_item(uid, ItemKeys.PRESTIGE_POINTS)
    user_arts = Artefacts.find(uid)

    # Calculate unlock cost
    unlock_cost = formulas.next_artefact_cost(len(user_arts))

    # Cannot afford the artefact, or limit has been reached
    if len(user_arts) >= len(artefacts) or (unlock_cost > points):
        raise HTTPException(400, {"error": "Max artefacts reached or cannot afford cost"})

    # Use sets to get a random new artefact id
    new_art_id = random.choice(list(set(list(artefacts.keys())) - set(list(user_arts.keys()))))

    if Artefacts.find_one(uid, new_art_id) is not None:
        raise HTTPException(400, {"error": "Artefact already unlocked"})

    # Insert the new artefact document
    Artefacts.insert_one({"userId": uid, "artefactId": new_art_id, "level": 1})

    # Update the purchase currency
    items = mongo.items.update_and_find(uid, {"$inc": {ItemKeys.PRESTIGE_POINTS: -unlock_cost}})

    return ServerResponse({
            "userItems": items,
            "userArtefacts": Artefacts.find(uid),
            "newArtefactId": new_art_id
         })


def _artefact_upgrade_cost(art: ArtefactData, level: int, buying: int):
    """ Calculate the upgrade cost

    Args:
        art (ArtefactData): Static game data for the artefact
        level (int): Current artefact level
        buying (int): Levels the user intended to upgrade

    Returns:
        int: Upgrade cost
    """
    return formulas.levelup_artefact_cost(art.cost_coeff, art.cost_expo, level, buying)
