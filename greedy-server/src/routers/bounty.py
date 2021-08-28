import math

import datetime as dt

from fastapi import APIRouter, HTTPException

from src.checks import user_or_raise
from src.common.enums import ItemKey
from src.routing import ServerRoute, ServerResponse
from src.models import UserIdentifier, ActiveBountyUpdateModel

from src import resources
from src.dataloader import MongoController

router = APIRouter(prefix="/api/bounty", route_class=ServerRoute)


@router.post("/claim")
async def claim_points(user: UserIdentifier):
    uid = user_or_raise(user)

    with MongoController() as mongo:
        u_bounty_data = await mongo.bounties.get_data(uid)

        u_bounties = u_bounty_data.get("bounties", {})
        u_last_claim = u_bounty_data.get("lastClaimTime", now := dt.datetime.utcnow())

        unclaimed = calc_unclaimed_total(u_bounties, u_last_claim, now)

        if unclaimed == 0:  # Stop the request here since any further would be a waste of time
            raise HTTPException(400, detail="Claim amount is zero")

        await mongo.bounties.set_claim_time(uid, now)

        u_items = await mongo.items.update_and_get(uid, {"$inc": {ItemKey.BOUNTY_POINTS: unclaimed}})

    return ServerResponse({"claimTime": now, "userItems": u_items, "pointsClaimed": unclaimed})


@router.post("/setactive")
async def set_active_bounties(data: ActiveBountyUpdateModel):
    uid = user_or_raise(data)

    with MongoController() as mongo:
        u_bounties = await mongo.bounties.get_user_bounties(uid)

        if not all(id_ in u_bounties for id_ in data.bounty_ids):
            raise HTTPException(400, detail="Attempting to set an invalid bounty")

        await mongo.bounties.update_active_bounties(uid, data.bounty_ids)

        u_bounties = await mongo.bounties.get_user_bounties(uid)

    return ServerResponse({"userBounties": u_bounties})


def calc_unclaimed_total(u_bounties, claim_time, now) -> int:

    bounty_res = resources.get_bounty_data()

    points = 0  # Total unclaimed points (ready to be claimed)

    # Interate over each bounty available
    for key, state in u_bounties.items():
        data = bounty_res.bounties[key]

        if state.get("isActive"):
            # Num. hours since the user has claimed this bounty
            total_hours = (now - claim_time).total_seconds() / 3_600

            # Clamp between 0 - max_unclaimed_hours
            hours_clamped = max(0, min(bounty_res.max_unclaimed_hours, total_hours))

            # Calculate the income and increment the total
            points += math.floor(hours_clamped * data.income)

    return points
