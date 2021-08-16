
from fastapi import APIRouter

from src.common import formulas
from src.common.enums import ItemKey
from src.checks import user_or_raise
from src.routing import ServerRoute, ServerResponse, ServerRequest
from src.models import UserIdentifier

from src import resources

router = APIRouter(prefix="/api", route_class=ServerRoute)


# Models
class PrestigeData(UserIdentifier):
    prestige_stage: int


@router.post("/prestige")
async def prestige(req: ServerRequest, data: PrestigeData):
    uid = user_or_raise(data)

    u_arts = await req.mongo.artefacts.get_all(uid)

    points_gained = formulas.stage_prestige_points(data.prestige_stage, u_arts)

    await process_new_bounties(req.mongo, uid, data.prestige_stage)

    # Increment the points with the gained amount
    await req.mongo.user_items.update_items(
        uid, {"$inc": {ItemKey.PRESTIGE_POINTS: points_gained}}
    )

    return ServerResponse({"completeUserData": await req.mongo.get_user_data(uid)})


async def process_new_bounties(mongo, uid, stage):
    bounty_res = resources.get_bounty_data()

    u_bounties = await mongo.user_bounties.get_user_bounties(uid)

    for key, bounty in bounty_res.bounties.items():
        if key not in u_bounties and stage >= bounty.stage:
            await mongo.user_bounties.add_new_bounty(uid, key)
