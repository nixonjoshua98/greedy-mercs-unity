
from fastapi import APIRouter

from src.common import formulas
from src.common.enums import ItemKey
from src.checks import user_or_raise
from src.routing import ServerRoute, ServerResponse
from src.models import UserIdentifier

from src import resources
from src.dataloader import MongoController

router = APIRouter(prefix="/api", route_class=ServerRoute)


# Models
class PrestigeData(UserIdentifier):
    prestige_stage: int


@router.post("/prestige")
async def prestige(data: PrestigeData):
    uid = user_or_raise(data)

    with MongoController() as mongo:

        await process_prestige_points(uid, data, mongo=mongo)
        await process_new_bounties(uid, data, mongo=mongo)

        u_data = await mongo.get_user_data(uid)

    return ServerResponse({"userData": u_data})


async def process_prestige_points(uid, req_data: PrestigeData, *, mongo: MongoController):

    u_artefacts = await mongo.artefacts.get_all_artefacts(uid)

    points_gained = formulas.stage_prestige_points(req_data.prestige_stage, u_artefacts)

    await mongo.items.update_items(uid, {"$inc": {ItemKey.PRESTIGE_POINTS: points_gained}})


async def process_new_bounties(uid, req_data: PrestigeData, *, mongo: MongoController):
    bounty_res = resources.get_bounty_data()

    u_bounties = await mongo.bounties.get_user_bounties(uid)

    for key, bounty in bounty_res.bounties.items():
        if key not in u_bounties and req_data.prestige_stage >= bounty.stage:
            await mongo.bounties.add_new_bounty(uid, key)
