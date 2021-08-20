import math

import datetime as dt

from fastapi import APIRouter, HTTPException

from src.checks import user_or_raise
from src.common.enums import ItemKey
from src.routing import ServerRoute, ServerResponse
from src.models import UserIdentifier

from src import resources
from src.dataloader import MongoController

router = APIRouter(prefix="/api/bounty", route_class=ServerRoute)


@router.post("/claimpoints")
async def claim_points(user: UserIdentifier):
    uid = user_or_raise(user)

    with MongoController() as mongo:
        u_bounties = await mongo.bounties.get_user_bounties(uid)

        unclaimed = calc_unclaimed_total(u_bounties, now := dt.datetime.utcnow())

        if unclaimed == 0:  # Stop the request here since any further would be a waste of time
            raise HTTPException(400, detail="Claim amount is zero")

        await mongo.bounties.set_all_claim_time(uid, now)

        u_items = await mongo.items.update_and_get(uid, {"$inc": {ItemKey.BOUNTY_POINTS: unclaimed}})

    return ServerResponse({"claimTime": now, "userItems": u_items, "pointsClaimed": unclaimed})


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
