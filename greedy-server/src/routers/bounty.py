import math

import datetime as dt

from fastapi import APIRouter, HTTPException, Depends

from src import resources
from src.checks import user_or_raise
from src.routing import ServerRoute, ServerResponse
from src.models import UserIdentifier, ActiveBountyUpdateModel
from src.common.enums import ItemKey
from src.dataloader import DataLoader
from src.mongo.models import UserBountiesModel
from src.mongo.repositories import BountiesRepository, bounties_repository

router = APIRouter(prefix="/api/bounty", route_class=ServerRoute)


@router.post("/claim")
async def claim_points(
        # Request Models = #
        user: UserIdentifier,
        # = Dependencies = #
        bounties_repo: BountiesRepository = Depends(bounties_repository)
):
    uid = await user_or_raise(user)

    # Load data from mongo
    bounties_user_data: UserBountiesModel = await bounties_repo.get_user(uid)

    # Calculate the unclaimed points
    unclaimed = calc_unclaimed_points(bounties_user_data, now := dt.datetime.utcnow())

    # Update the claim time
    await bounties_repo.set_claim_time(uid, claim_time=now)

    u_items = await DataLoader().items.update_and_get(uid, {"$inc": {ItemKey.BOUNTY_POINTS: unclaimed}})

    return ServerResponse({"claimTime": now, "userItems": u_items, "pointsClaimed": unclaimed})


@router.post("/setactive")
async def set_active_bounties(
        # Request Models = #
        data: ActiveBountyUpdateModel,
        # = Dependencies = #
        bounties_repo: BountiesRepository = Depends(bounties_repository)
):
    uid = await user_or_raise(data)

    # Check that the user is attempting to activate an acceptable num of bounties
    check_num_active_bounties(data)

    # Load data from the mongo database
    bounties_user_data: UserBountiesModel = await bounties_repo.get_user(uid)

    # Confirm that the user has the bounty unlocked
    check_unlocked_bounty(data, bounties_user_data)

    # Enable (or disable) the relevant bounties
    await bounties_repo.set_active_bounties(uid, data.bounty_ids)

    # Refresh the user data from the database, ready to return it back to the user
    bounties_user_data: UserBountiesModel = await bounties_repo.get_user(uid)

    return ServerResponse(bounties_user_data.response_dict())


# === Calculations === #

def calc_unclaimed_points(user_data: UserBountiesModel, now: dt.datetime) -> int:
    bounty_res = resources.get_bounty_data()

    points = 0  # Total unclaimed points (ready to be claimed)

    # Interate over each active bounty available
    for bounty in user_data.active_bounties:
        data = bounty_res.bounties[bounty.bounty_id]

        # Num. hours since the user has claimed this bounty
        total_hours = (now - user_data.last_claim_time).total_seconds() / 3_600

        # Clamp between 0 - max_unclaimed_hours
        hours_clamped = max(0, min(bounty_res.max_unclaimed_hours, total_hours))

        # Calculate the income and increment the total
        points += math.floor(hours_clamped * data.income)

    return points


# === Request Checks === #

def check_num_active_bounties(data: ActiveBountyUpdateModel):
    res_bounties = resources.get_bounty_data()

    # Check that the user is attempting to activate an allowed num of bounties
    if len(data.bounty_ids) > res_bounties.max_active_bounties:
        raise HTTPException(400, detail="Too many active bounties")

    return True


def check_unlocked_bounty(req_model: ActiveBountyUpdateModel, user_data: UserBountiesModel):
    # Grab the bounty ids which the user has unlocked
    user_bounty_ids = [b.bounty_id for b in user_data.bounties]

    # User is attempting to use a bounty they do not have unlocked
    if not all(id_ in user_bounty_ids for id_ in req_model.bounty_ids):
        raise HTTPException(400, detail="Attempting to allow a locked bounty")

    return True
