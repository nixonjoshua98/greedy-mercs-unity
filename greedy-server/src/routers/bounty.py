import math

import datetime as dt

from fastapi import APIRouter, Request

from src.checks import user_or_raise
from src.common.enums import ItemKeys
from src.routing import CustomRoute, ServerResponse
from src.models import UserIdentifier

from src import resources

from src.db.queries import (
    bounties as BountyQueries,
    useritems as UserItemsQueries
)

router = APIRouter(prefix="/api/bounty", route_class=CustomRoute)


@router.post("/claimpoints")
async def claim_points(req: Request, user: UserIdentifier):
    uid = user_or_raise(user)

    # Load user data from database
    user_bounty_data = await BountyQueries.get_user_bounties(req.state.mongo, uid)

    # Calculate the unclaimed points from bounties
    unclaimed = calc_unclaimed_total(user_bounty_data, now := dt.datetime.utcnow())

    # Set the claim time of user bounties
    await BountyQueries.set_all_claim_time(req.state.mongo, uid, now)

    # Add the points and return all user items
    u_items = await UserItemsQueries.update_and_get_items(
        req.state.mongo, uid, {"$inc": {ItemKeys.BOUNTY_POINTS: unclaimed}}
    )

    return ServerResponse({"claimTime": now, "userItems": u_items})


def calc_unclaimed_total(user_bounty_data, now) -> int:

    bounty_res = resources.get_bounty_data()

    points = 0  # Total unclaimed points (ready to be claimed)

    # Interate over each bounty available
    for key, state in user_bounty_data.items():
        data = bounty_res.bounties[key]

        # Num. hours since the user has claimed this bounty
        total_hours = (now - state["lastClaimTime"]).total_seconds() / 3_600

        # Clamp between 0 - max_unclaimed_hours
        hours_clamped = max(0, min(bounty_res.max_unclaimed_hours, total_hours))

        # Calculate the income and increment the total
        points += math.floor(hours_clamped * data.income)

    return points
