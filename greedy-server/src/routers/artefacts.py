import random

from fastapi import APIRouter, HTTPException

from src import svrdata
from src.svrdata import Items
from src.checks import user_or_raise
from src.common import formulas, resources, mongo
from src.routing import CustomRoute, ServerResponse
from src.models import UserIdentifier

router = APIRouter(prefix="/api/artefact", route_class=CustomRoute)


# Models
class ArtefactUpgradeModel(UserIdentifier):
    artefact_id: int
    purchase_levels: int


@router.post("/upgrade")
def upgrade(data: ArtefactUpgradeModel):
    uid = user_or_raise(data)

    static_art = resources.get("artefacts")[data.artefact_id]

    user_art = svrdata.artefacts.get_one_artefact(uid, data.artefact_id)

    # Calculate the level up cost
    cost = formulas.levelup_artefact_cost(
        static_art["costCoeff"],
        static_art["costExpo"],
        user_art["level"],
        data.purchase_levels
    )

    # Generic all rounded check if the upgrade can go ahead
    if not can_upgrade_artefact(uid, static_art, user_art, cost, data.purchase_levels):
        raise HTTPException(400)

    items = Items.find_and_update_one({"userId": uid}, {"$inc": {"prestigePoints": -cost}})

    update_artefact(uid, data.artefact_id, inc={"level": data.purchase_levels})

    return ServerResponse(
        {
            "userItems": items,
            "userArtefacts": svrdata.artefacts.get_all_artefacts(uid, as_dict=True)
        }
    )


@router.post("/unlock")
def unlock(data: UserIdentifier):
    uid = user_or_raise(data)

    static_arts = resources.get("artefacts")

    user_arts = svrdata.artefacts.get_all_artefacts(uid, as_dict=True)

    cost = formulas.next_artefact_cost(len(user_arts))

    # Simple purchase check
    if not can_purchase_artefact(uid, user_arts, static_arts, cost):
        raise HTTPException(400)

    new_art_id = random.choice(list(set(list(static_arts.keys())) - set(list(user_arts.keys()))))

    unlock_artefact(uid, new_art_id)  # Unlock the artefact, may throw an error if the artefact already exists

    items = Items.find_and_update_one({"userId": uid}, {"$inc": {"prestigePoints": -cost}})

    return ServerResponse(
        {
            "userItems": items,
            "userArtefacts": svrdata.artefacts.get_all_artefacts(uid, as_dict=True),
            "newArtefactId": new_art_id
         }
    )


def can_upgrade_artefact(uid, static_art, user_art, cost, levels):

    # User does not own the artefact or the level will exceeed the max level
    if user_art is None or (user_art["level"] + levels) > static_art.get("maxLevel", float("inf")):
        return False

    points = Items.find_one({"userId": uid}).get(Items.PRESTIGE_POINTS, 0)

    return points >= cost


def can_purchase_artefact(uid, user_arts, all_static_arts, cost):

    points = Items.find_one({"userId": uid}).get(Items.PRESTIGE_POINTS, 0)

    if len(user_arts) >= len(all_static_arts) or (cost > points):
        return False

    return True


# Database
def update_artefact(uid, iid, *, inc: dict, upsert: bool = True) -> bool:
    result = mongo.db["userArtefacts"].update_one({"userId": uid, "artefactId": iid}, {"$inc": inc}, upsert=upsert)

    return result.modified_count == 1


def unlock_artefact(uid, iid):

    if svrdata.artefacts.get_one_artefact(uid, iid) is not None:
        raise HTTPException(400, {"error": "Artefact already unlocked"})

    mongo.db["userArtefacts"].insert_one({"userId": uid, "artefactId": iid, "level": 1})

