import random

from fastapi import APIRouter, HTTPException

from src import resources
from src.resources import ArtefactData
from src.checks import user_or_raise
from src.common import formulas
from src.common.enums import ItemKey
from src.routing import ServerRoute, ServerResponse, ServerRequest
from src.models import UserIdentifier

router = APIRouter(prefix="/api/artefact", route_class=ServerRoute)


# Models
class ArtefactUpgradeModel(UserIdentifier):
    artefact_id: int
    purchase_levels: int


@router.post("/upgrade")
async def upgrade(req: ServerRequest, data: ArtefactUpgradeModel):
    uid = user_or_raise(data)

    # Load the artefact resource
    artefact: ArtefactData = resources.get_artefacts().artefacts[data.artefact_id]

    u_art = await req.mongo.artefacts.get_one(uid, data.artefact_id)

    # Pull the data and raise an error if it does not exist
    if u_art is None:
        raise HTTPException(400, detail="Artefact is not unlocked")

    # Upgrading will exceed the max level
    elif (u_art["level"] + data.purchase_levels) > artefact.max_level:
        raise HTTPException(400, detail="Level will exceed max level")

    # Calculate the upgrade cost, and pull the currency from the database
    cost = artefact.upgrade_cost(u_art["level"], data.purchase_levels)

    u_pp = await req.mongo.user_items.get_item(uid, ItemKey.PRESTIGE_POINTS)

    # Perform the upgrade check (and calculate the upgrade cost)
    if cost > u_pp:  # Raise a HTTP error so the request is aborted
        raise HTTPException(400, {"error": "Cannot afford upgrade cost"})

    # Update the artefact (and pull the new artefact data)
    u_items = await req.mongo.user_items.update_and_get(
        uid, {"$inc": {ItemKey.PRESTIGE_POINTS: -cost}}
    )

    await req.mongo.artefacts.update_one(
        uid, data.artefact_id, {"$inc": {"level": data.purchase_levels}}
    )

    return ServerResponse(
        {
            "userItems": u_items,
            "userArtefacts": await req.mongo.artefacts.get_all(uid)
        }
    )


@router.post("/unlock")
async def unlock(req: ServerRequest, data: UserIdentifier):
    uid = user_or_raise(data)

    artefacts = resources.get_artefacts().artefacts

    # Pull user data from the database
    u_pp = await req.mongo.user_items.get_item(uid, ItemKey.PRESTIGE_POINTS)

    u_arts = await req.mongo.artefacts.get_all(uid)

    # Calculate unlock cost
    unlock_cost = formulas.next_artefact_cost(len(u_arts))

    # Cannot afford the artefact, or limit has been reached
    if len(u_arts) >= len(artefacts) or (unlock_cost > u_pp):
        raise HTTPException(400, detail="Max artefacts reached or cannot afford cost")

    # Use sets to get a random new artefact id
    new_art_id = random.choice(list(set(list(artefacts.keys())) - set(list(u_arts.keys()))))

    # We UPSERT the new artefact (We could INSERT but this prevents multiple entries - last error check)
    await req.mongo.artefacts.update_one(uid, new_art_id, {"$setOnInsert": {"level": 1}})

    u_arts = req.mongo.artefacts.get_all(uid)

    # Update the purchase currency
    u_items = await req.mongo.user_items.update_and_get(
        uid, {"$inc": {ItemKey.PRESTIGE_POINTS: -unlock_cost}}
    )

    return ServerResponse(
        {
            "userItems": u_items,
            "userArtefacts": u_arts,
         }
    )
