
from fastapi import APIRouter, HTTPException

from typing import Tuple

from src import resources
from src.checks import user_or_raise
from src.svrdata import Armoury, Items
from src.routing import CustomRoute, ServerResponse
from src.models import UserIdentifier

router = APIRouter(prefix="/api/armoury", route_class=CustomRoute)


# Models
class ItemPurchaseModel(UserIdentifier):
    item_id: int


@router.post("/upgrade")
def upgrade(data: ItemPurchaseModel):
    uid = user_or_raise(data)

    cost, can_level = can_levelup(uid, data.item_id)

    if not can_level:
        raise HTTPException(400, {"error": "Cannot upgrade item"})

    modified = Armoury.update_one({"userId": uid, "itemId": data.item_id}, {"$inc": {"level": 1}}, upsert=False)

    if not modified:  # Return an error if a document was not modified
        raise HTTPException(400)

    u_items = Items.find_and_update_one({"userId": uid}, {"$inc": {"armouryPoints": -cost}})

    return ServerResponse({
            "userArmouryItems": Armoury.find({"userId": uid}), "userArmouryPoints": u_items["armouryPoints"]
        })


@router.post("/evolve")
def evolve(data: ItemPurchaseModel):
    uid = user_or_raise(data)

    cost, can_level = can_evolve(uid, data.item_id)

    if not can_level:
        raise HTTPException(400, {"error": "Cannot evolve item"})

    modified = Armoury.update_one(
        {"userId": uid, "itemId": data.item_id}, {"$inc": {"evoLevel": 1, "owned": -cost}}, upsert=False
    )

    if not modified:  # Return an error if a document was not modified
        raise HTTPException(400)

    return ServerResponse({"userArmouryItems": Armoury.find({"userId": uid})})


def can_evolve(uid, iid) -> Tuple[int, bool]:
    armoury = resources.get_armoury()

    if (item := Armoury.find_one({"userId": uid, "itemId": iid})) is None:
        return -1, False

    within_max_level = item.get("evoLevel", 0) < armoury.max_evo_level
    enough_copies = item.get("owned", 0) >= (armoury.evo_level_cost + 1)

    return armoury.evo_level_cost, within_max_level and enough_copies


def can_levelup(uid, iid) -> Tuple[int, bool]:
    armoury = resources.get_armoury()

    if (item := Armoury.find_one({"userId": uid, "itemId": iid})) is None:
        return -1, False

    level_cost = armoury.items[iid].level_cost(item["level"])

    ap_points = Items.find_one({"userId": uid}).get("armouryPoints", 0)

    return level_cost, ap_points >= level_cost
