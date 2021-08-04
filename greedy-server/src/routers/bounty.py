import math

import datetime as dt

from fastapi import APIRouter

from src.checks import user_or_raise
from src.common.enums import ItemKeys
from src.routing import CustomRoute, ServerResponse
from src.models import UserIdentifier

from src import database as db
from src import resources

router = APIRouter(prefix="/api/bounty", route_class=CustomRoute)


@router.post("/claimpoints")
def claim_points(user: UserIdentifier):
    uid = user_or_raise(user)

    unclaimed = calc_unclaimed_total(uid, now := dt.datetime.utcnow())

    # Update the claim time for each bounty
    db.bounties.set_all_claim_time(uid, claim_time=now)

    # Add the bounty points to the users inventory
    items = db.items.update_and_get_user_items(uid, {"$inc": {ItemKeys.BOUNTY_POINTS: unclaimed}})

    return ServerResponse({"claimTime": now, "userItems": items})


def calc_unclaimed_total(uid, now) -> int:

    bounty_res = resources.get_bounty_data()

    user_bounties = db.bounties.get_user_bounties(uid)

    points = 0  # Total unclaimed points (ready to be claimed)

    # Interate over each bounty available
    for key, state in user_bounties.items():
        data = bounty_res.bounties[key]

        # Num. hours since the user has claimed this bounty
        total_hours = (now - state["lastClaimTime"]).total_seconds() / 3_600

        # Clamp between 0 - max_unclaimed_hours
        hours_clamped = max(0, min(bounty_res.max_unclaimed_hours, total_hours))

        # Calculate the income and increment the total
        points += math.floor(hours_clamped * data.income)

    return points
