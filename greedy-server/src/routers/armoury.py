
from fastapi import APIRouter, HTTPException

from typing import Tuple

from src import resources
from src.common.enums import ItemKeys
from src.checks import user_or_raise
from src.routing import ServerRoute, ServerResponse, ServerRequest
from src.models import UserIdentifier
from src.dataloader.client import DataLoader

router = APIRouter(prefix="/api/armoury", route_class=ServerRoute)


# Models
class ItemPurchaseModel(UserIdentifier):
    item_id: int


@router.post("/upgrade")
async def upgrade(req: ServerRequest, data: ItemPurchaseModel):
    uid = user_or_raise(data)

    # Check ability to level up (and return the upgrade cost)
    cost, can_level = await can_levelup(req.mongo, uid, data.item_id)

    if not can_level:
        raise HTTPException(400, detail="Cannot level up")

    doc_modified = await req.mongo.armoury.update_one(
        uid, data.item_id, {"$inc": {"level": 1}}, upsert=False
    )

    if not doc_modified:  # Return an error if a document was not modified
        raise HTTPException(400, detail="Error")

    # Deduct the upgrade cost and return all user items AFTER the update
    u_items = await req.mongo.user_items.update_and_get(
        uid, {"$inc": {ItemKeys.ARMOURY_POINTS: -cost}}
    )

    return ServerResponse({"userArmouryItems": await req.mongo.armoury.get_all_user_items(uid), "userItems": u_items})


@router.post("/evolve")
async def evolve(req: ServerRequest, data: ItemPurchaseModel):
    uid = user_or_raise(data)

    cost, can_level = await can_evolve(req.mongo, uid, data.item_id)

    if not can_level:
        raise HTTPException(400, detail="Cannot evolve item")

    doc_modified = await req.mongo.armoury.update_one(
        uid, data.item_id, {"$inc": {"evoLevel": 1, "owned": -cost}}, upsert=False
    )

    if not doc_modified:  # Return an error if a document was not modified
        raise HTTPException(400, detail="Error")

    return ServerResponse({"userArmouryItems": await req.mongo.armoury.get_all_user_items(uid)})


async def can_evolve(mongo: DataLoader, uid, iid) -> Tuple[int, bool]:
    armoury = resources.get_armoury()

    u_armoury_item = await mongo.armoury.get_one_user_item(uid, iid)

    if u_armoury_item is None:
        return -1, False

    within_max_level = u_armoury_item.get("evoLevel", 0) < armoury.max_evo_level
    enough_copies = u_armoury_item.get("owned", 0) >= (armoury.evo_level_cost + 1)

    return armoury.evo_level_cost, within_max_level and enough_copies


async def can_levelup(mongo: DataLoader, uid, iid) -> Tuple[int, bool]:
    armoury = resources.get_armoury()

    u_armoury_item = await mongo.armoury.get_one_user_item(uid, iid)

    if u_armoury_item is None:
        return -1, False

    level_cost = armoury.items[iid].level_cost(u_armoury_item["level"])

    u_ap = await mongo.user_items.get_item(uid, ItemKeys.ARMOURY_POINTS)

    return level_cost, u_ap >= level_cost
