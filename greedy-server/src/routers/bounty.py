import math

import datetime as dt

from fastapi import APIRouter

from src.svrdata import Items
from src.checks import user_or_raise
from src.common import mongo, resources
from src.routing import CustomRoute, ServerResponse
from src.models import UserIdentifier

router = APIRouter(prefix="/api/bounty", route_class=CustomRoute)


@router.post("/claimpoints")
def claim_points(user: UserIdentifier):
    uid = user_or_raise(user)

    unclaimed = calc_unclaimed_total(uid, now := dt.datetime.utcnow())

    # Update the claim time for each bounty
    mongo.db["userBounties"].update_many({"userId": uid}, {"$set": {"lastClaimTime": now}})

    # Add the bounty points to the users inventory
    items = Items.find_and_update_one({"userId": uid}, {"$inc": {"bountyPoints": unclaimed}})

    return ServerResponse({"claimTime": now, "userItems": items})


def calc_unclaimed_total(uid, now) -> int:

    # bounties.json file
    bounty_data_file = resources.get("bounties")

    bounties_svr_data = bounty_data_file["bounties"]
    max_unclaimed_hours = bounty_data_file["maxUnclaimedHours"]

    # Load the users bounties into a List
    user_bounties = {b["bountyId"]: b for b in list(mongo.db["userBounties"].find({"userId": uid}))}

    points = 0  # Total unclaimed points (ready to be claimed)

    # Interate over each bounty available
    for key, bounty_data in bounties_svr_data.items():

        # User has not unlocked this bounty
        if (bounty_entry := user_bounties.get(key)) is None:
            continue

        # Num. hours since the user has claimed this bounty (Note: From the database, not the max allowed)
        hours_since_claim = (now - bounty_entry["lastClaimTime"]).total_seconds() / 3_600

        # Hours since bounty claimed, taking into account the max unclaimed hours
        hours = max(0, min(max_unclaimed_hours, hours_since_claim))

        # Calculate the income and increment the total
        points += math.floor(hours * bounty_data["hourlyIncome"])

    return points
