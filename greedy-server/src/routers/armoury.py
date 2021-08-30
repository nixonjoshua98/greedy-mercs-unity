
from fastapi import APIRouter, HTTPException

from src.common.enums import ItemKey
from src.checks import user_or_raise
from src.routing import ServerRoute, ServerResponse
from src.models import ArmouryItemActionModel

from src.dataloader import DataLoader
from src import resources

router = APIRouter(prefix="/api/armoury", route_class=ServerRoute)


@router.post("/upgrade")
async def upgrade(data: ArmouryItemActionModel):
    uid = await user_or_raise(data)

    armoury = resources.get_armoury_resources()

    with DataLoader() as mongo:

        if (u_armoury_item := await mongo.armoury.get_one_item(uid, data.item_id)) is None:
            raise HTTPException(400, detail="Item does not exist")

        u_ap = await mongo.items.get_item(uid, ItemKey.ARMOURY_POINTS)

        upgrade_cost = armoury.items[data.item_id].level_cost(u_armoury_item["level"])

        # Throw an error if the user cannot afford the upgrade
        if upgrade_cost > u_ap:
            raise HTTPException(400, detail="Cannot afford upgrade")

        # Attempt to upgrade the item
        doc_modified = await mongo.armoury.update_one_item(uid, data.item_id, {"$inc": {"level": 1}}, upsert=False)

        # No item document was modied (implies we are upgrading an item which does not exist)
        if not doc_modified:  # Return an error if a document was not modified
            raise HTTPException(400, detail="Error")

        # Deduct the upgrade cost and return all user items AFTER the update
        u_items = await mongo.items.update_and_get(uid, {"$inc": {ItemKey.ARMOURY_POINTS: -upgrade_cost}})

        u_armoury = await mongo.armoury.get_all_items(uid)

    return ServerResponse({"userArmouryItems": u_armoury, "userItems": u_items})


@router.post("/evolve")
async def evolve(data: ArmouryItemActionModel):
    uid = await user_or_raise(data)

    armoury = resources.get_armoury_resources()

    with DataLoader() as mongo:

        # Attempt to pull the item, otherwise throw an error
        if (u_armoury_item := await mongo.armoury.get_one_item(uid, data.item_id)) is None:
            raise HTTPException(400, detail="Item does not exist")

        # Checks
        within_max_level = u_armoury_item.get("evoLevel", 0) < armoury.max_evo_level
        enough_copies = u_armoury_item.get("owned", 0) >= (armoury.evo_level_cost + 1)

        # Throw an error if the user/item does not meet the requirements
        if not (within_max_level and enough_copies):  # Item does not meet the requirements
            raise HTTPException(400, detail="Cannot evolve item")

        # Update the item but do not upsert, one last error check to ensure the item exists
        doc_modified = await mongo.armoury.update_one_item(
            uid, data.item_id, {"$inc": {"evoLevel": 1, "owned": -armoury.evo_level_cost}}, upsert=False
        )

        if not doc_modified:  # Return an error if a document was not modified
            raise HTTPException(400, detail="Error")

        u_armoury = await mongo.armoury.get_all_items(uid)

    return ServerResponse({"userArmouryItems": u_armoury})
