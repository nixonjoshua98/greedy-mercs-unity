
from fastapi import APIRouter

from src import svrdata
from src.svrdata import Artefacts
from src.common import formulas
from src.common.enums import ItemKeys
from src.checks import user_or_raise
from src.routing import CustomRoute, ServerResponse
from src.models import UserIdentifier

from src import database as db
from src import resources

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
    db.items.update_user_items(uid, {"$inc": {ItemKeys.PRESTIGE_POINTS: points_gained}})

    return ServerResponse({"completeUserData": svrdata.get_player_data(uid)})


def process_new_bounties(uid, stage):
    user_bounties = db.bounties.get_user_bounties(uid)

    bounty_res = resources.get_bounty_data()

    def new_earned_bounty(id_, b):
        return id_ not in user_bounties and stage >= b.stage

    for key, bounty in bounty_res.bounties.items():
        if new_earned_bounty(key, bounty):
            db.bounties.add_new_bounty(uid, key)
