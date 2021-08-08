
from fastapi import APIRouter, Request

from src import svrdata
from src.svrdata import Artefacts
from src.common import formulas
from src.common.enums import ItemKeys
from src.checks import user_or_raise
from src.routing import CustomRoute, ServerResponse
from src.models import UserIdentifier

from src.db.queries import (
    bounties as BountyQueries,
    useritems as UserItemsQueries
)

from src import resources

router = APIRouter(prefix="/api", route_class=CustomRoute)


# Models
class PrestigeData(UserIdentifier):
    prestige_stage: int


@router.post("/prestige")
async def prestige(req:  Request, data: PrestigeData):
    uid = user_or_raise(data)

    user_artefacts = Artefacts.find(uid)

    points_gained = formulas.stage_prestige_points(data.prestige_stage, user_artefacts)

    await process_new_bounties(uid, req.state.mongo, data.prestige_stage)

    # Increment the points with the gained amount
    await UserItemsQueries.update_items(
        req.state.mongo, uid, {"$inc": {ItemKeys.PRESTIGE_POINTS: points_gained}}
    )

    return ServerResponse({"completeUserData": await svrdata.get_player_data(req.state.mongo, uid)})


async def process_new_bounties(uid, mongo, stage):
    bounty_res = resources.get_bounty_data()

    user_bounty_data = await BountyQueries.get_user_bounties(mongo, uid)

    for key, bounty in bounty_res.bounties.items():
        if key not in user_bounty_data and stage >= bounty.stage:
            await BountyQueries.add_new_bounty(mongo, uid, key)
