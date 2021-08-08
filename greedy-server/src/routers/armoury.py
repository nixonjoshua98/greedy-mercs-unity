
from fastapi import APIRouter, HTTPException, Request

from typing import Tuple

from src import resources
from src.common.enums import ItemKeys
from src.checks import user_or_raise

from src.svrdata import Armoury

from src.routing import CustomRoute, ServerResponse
from src.models import UserIdentifier

from src.db.queries import (
    useritems as UserItemsQueries
)

router = APIRouter(prefix="/api/armoury", route_class=CustomRoute)


# Models
class ItemPurchaseModel(UserIdentifier):
    item_id: int


@router.post("/upgrade")
async def upgrade(req: Request, data: ItemPurchaseModel):
    uid = user_or_raise(data)

    # Check ability to level up (and return the upgrade cost)
    cost, can_level = await can_levelup(req.state.mongo, uid, data.item_id)

    if not can_level:
        raise HTTPException(400, detail="Cannot level up")

    modified = Armoury.update_one({"userId": uid, "itemId": data.item_id}, {"$inc": {"level": 1}}, upsert=False)

    if not modified:  # Return an error if a document was not modified
        raise HTTPException(400, detail="Error")

    # Deduct the upgrade cost and return all user items AFTER the update
    u_items = await UserItemsQueries.update_and_get_items(
        req.state.mongo, uid, {"$inc": {ItemKeys.ARMOURY_POINTS: -cost}}
    )

    return ServerResponse({"userArmouryItems": Armoury.find({"userId": uid}), "userItems": u_items})


@router.post("/evolve")
async def evolve(data: ItemPurchaseModel):
    uid = user_or_raise(data)

    cost, can_level = can_evolve(uid, data.item_id)

    if not can_level:
        raise HTTPException(400, detail="Cannot evolve item")

    modified = Armoury.update_one(
        {"userId": uid, "itemId": data.item_id}, {"$inc": {"evoLevel": 1, "owned": -cost}}, upsert=False
    )

    if not modified:  # Return an error if a document was not modified
        raise HTTPException(400, detail="Error")

    return ServerResponse({"userArmouryItems": Armoury.find({"userId": uid})})


def can_evolve(uid, iid) -> Tuple[int, bool]:
    armoury = resources.get_armoury()

    if (item := Armoury.find_one({"userId": uid, "itemId": iid})) is None:
        return -1, False

    within_max_level = item.get("evoLevel", 0) < armoury.max_evo_level
    enough_copies = item.get("owned", 0) >= (armoury.evo_level_cost + 1)

    return armoury.evo_level_cost, within_max_level and enough_copies


async def can_levelup(mongo, uid, iid) -> Tuple[int, bool]:
    armoury = resources.get_armoury()

    if (item := Armoury.find_one({"userId": uid, "itemId": iid})) is None:
        return -1, False

    level_cost = armoury.items[iid].level_cost(item["level"])

    # Pull single user item from database
    points = await UserItemsQueries.get_item(mongo, uid, ItemKeys.ARMOURY_POINTS)

    return level_cost, points >= level_cost
