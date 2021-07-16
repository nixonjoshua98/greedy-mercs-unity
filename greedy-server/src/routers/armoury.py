
from fastapi import APIRouter, HTTPException

from src.checks import user_or_raise
from src.svrdata import Armoury
from src.routing import CustomRoute, ServerResponse
from src.models import UserIdentifier

router = APIRouter(prefix="/api/armoury", route_class=CustomRoute)


# Models
class ItemPurchaseModel(UserIdentifier):
    item_id: int


@router.post("/upgrade")
def upgrade(data: ItemPurchaseModel):
    uid = user_or_raise(data)

    modified = Armoury.update_one({"userId": uid, "itemId": data.item_id}, {"$inc": {"level": 1}}, upsert=False)

    # Return an error if a document was not modified
    if not modified:
        raise HTTPException(400)

    return ServerResponse({"userArmouryItems": Armoury.find({"userId": uid})})


@router.post("/evolve")
def evolve(data: ItemPurchaseModel):
    uid = user_or_raise(data)

    modified = Armoury.update_one({"userId": uid, "itemId": data.item_id}, {"$inc": {"evoLevel": 1}}, upsert=False)

    # Return an error if a document was not modified
    if not modified:
        raise HTTPException(400)

    return ServerResponse({"userArmouryItems": Armoury.find({"userId": uid})})
