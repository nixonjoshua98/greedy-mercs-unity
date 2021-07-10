
from fastapi import APIRouter, HTTPException

from src import svrdata
from src.checks import user_or_raise
from src.routing import CustomRoute, ServerResponse
from src.basemodels import UserIdentifier

router = APIRouter(prefix="/api/armoury", route_class=CustomRoute)


# Models
class ItemPurchaseModel(UserIdentifier):
    item_id: int


@router.post("/upgrade")
def upgrade(data: ItemPurchaseModel):
    uid = user_or_raise(data)

    doc_changed = svrdata.armoury.update_one_item(uid, data.item_id, inc={"level": 1}, upsert=False)

    if not doc_changed:  # Return an error if a document was not modified
        raise HTTPException(400)

    return ServerResponse({"userArmouryItems": svrdata.armoury.get_armoury(uid)})


@router.post("/evolve")
def evolve(data: ItemPurchaseModel):
    uid = user_or_raise(data)

    doc_changed = svrdata.armoury.update_one_item(uid, data.item_id, inc={"evoLevel": 1}, upsert=False)

    if not doc_changed:  # Return an error if a document was not modified
        raise HTTPException(400)

    return ServerResponse({"userArmouryItems": svrdata.armoury.get_armoury(uid)})
