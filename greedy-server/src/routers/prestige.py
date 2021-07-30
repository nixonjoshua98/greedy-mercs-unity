
from fastapi import APIRouter

import datetime as dt

from src import svrdata
from src.svrdata import Artefacts
from src.common import resources, formulas
from src.checks import user_or_raise
from src.routing import CustomRoute, ServerResponse
from src.models import UserIdentifier

from src.database import mongo, ItemKeys

router = APIRouter(prefix="/api", route_class=CustomRoute)


# Models
class PrestigeData(UserIdentifier):
    prestige_stage: int


@router.post("/prestige")
def prestige(data: PrestigeData):
    uid = user_or_raise(data)

    user_artefacts = Artefacts.find(uid)

    process_new_bounties(uid, data.prestige_stage)

    points_gained = formulas.stage_prestige_points(data.prestige_stage, user_artefacts)

    # Increment the user points with the gained amount
    mongo.items.update_one(uid, {"$inc": {ItemKeys.PRESTIGE_POINTS: points_gained}})

    return ServerResponse({"completeUserData": svrdata.get_player_data(uid)})


def process_new_bounties(uid, stage):
    now = dt.datetime.utcnow()

    user_bounties = svrdata.bounty.get_bounties(uid, as_dict=True)

    bounty_data = resources.get("bounties")["bounties"]

    def new_earned_bounty(id_, b):
        return id_ not in user_bounties and stage >= b["unlockStage"]

    for key, bounty in bounty_data.items():
        if new_earned_bounty(key, bounty):
            svrdata.bounty.insert_bounty(uid, key, now)
