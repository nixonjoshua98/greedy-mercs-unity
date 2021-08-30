import random

from fastapi import APIRouter, HTTPException

from src.resources import ArtefactResources
from src.checks import user_or_raise
from src.common import formulas
from src.common.enums import ItemKey
from src.routing import ServerRoute, ServerResponse
from src.models import UserIdentifier
from src.dataloader import DataLoader
from src import resources

router = APIRouter(prefix="/api/artefact", route_class=ServerRoute)


# Models
class ArtefactUpgradeModel(UserIdentifier):
    artefact_id: int
    purchase_levels: int


@router.post("/upgrade")
async def upgrade(data: ArtefactUpgradeModel):
    uid = await user_or_raise(data)

    # Load the artefact resource
    artefact: ArtefactResources = resources.get_artefacts_data().artefacts[data.artefact_id]

    with DataLoader() as mongo:

        # Pull the data and raise an error if it does not exist
        if (u_artefact := await mongo.artefacts.get_one_artefact(uid, data.artefact_id)) is None:
            raise HTTPException(400, detail="Artefact is not unlocked")

        # Upgrading will exceed the max level
        elif (u_artefact["level"] + data.purchase_levels) > artefact.max_level:
            raise HTTPException(400, detail="Level will exceed max level")

        # Calculate the upgrade cost, and pull the currency from the database
        cost = artefact.upgrade_cost(u_artefact["level"], data.purchase_levels)

        u_pp = await mongo.items.get_item(uid, ItemKey.PRESTIGE_POINTS)

        # Perform the upgrade check (and calculate the upgrade cost)
        if cost > u_pp:  # Raise a HTTP error so the request is aborted
            raise HTTPException(400, detail="Cannot afford upgrade cost")

        # Update the artefact (and pull the new artefact data)
        u_items = await mongo.items.update_and_get(uid, {"$inc": {ItemKey.PRESTIGE_POINTS: -cost}})

        await mongo.artefacts.update_one_artefact(uid, data.artefact_id, {"$inc": {"level": data.purchase_levels}})

        u_artefacts = await mongo.artefacts.get_all_artefacts(uid)

    return ServerResponse({"userItems": u_items, "userArtefacts": u_artefacts})


@router.post("/unlock")
async def unlock(data: UserIdentifier):
    uid = await user_or_raise(data)

    artefacts = resources.get_artefacts_data().artefacts

    with DataLoader() as mongo:

        u_pp = await mongo.items.get_item(uid, ItemKey.PRESTIGE_POINTS)
        u_artefacts = await mongo.artefacts.get_all_artefacts(uid)

        # Calculate unlock cost
        unlock_cost = formulas.next_artefact_cost(len(u_artefacts))

        # Cannot afford the artefact, or limit has been reached
        if len(u_artefacts) >= len(artefacts) or (unlock_cost > u_pp):
            raise HTTPException(400, detail="Max artefacts reached or cannot afford cost")

        # Use sets to get a random new artefact id
        u_new_art = random.choice(list(set(list(artefacts.keys())) - set(list(u_artefacts.keys()))))

        # We UPSERT the new artefact (We could INSERT but this prevents multiple entries - last error check)
        await mongo.artefacts.update_one_artefact(uid, u_new_art, {"$setOnInsert": {"level": 1}})

        # Update the purchase currency
        u_items = await mongo.items.update_and_get(uid, {"$inc": {ItemKey.PRESTIGE_POINTS: -unlock_cost}})

        u_artefacts = await mongo.artefacts.get_all_artefacts(uid)

    return ServerResponse({"userItems": u_items, "userArtefacts": u_artefacts, "newArtefactId": u_new_art})
