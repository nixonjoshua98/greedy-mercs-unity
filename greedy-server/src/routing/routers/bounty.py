import datetime as dt
import math

from fastapi import Depends, HTTPException

from src import utils
from src.mongo.repositories.bounties import (
    BountiesRepository,
    UserBountiesModel,
    inject_bounties_repository,
)
from src.mongo.repositories.currency import CurrencyRepository
from src.mongo.repositories.currency import Fields as CurrencyRepoFields
from src.mongo.repositories.currency import inject_currency_repository
from src.pymodels import BaseModel
from src.resources.bounties import StaticBounties, inject_static_bounties
from src.routing import APIRouter, ServerResponse
from src.routing.dependencies.authentication import (
    AuthenticatedUser,
    inject_authenticated_user,
)

router = APIRouter(prefix="/api/bounty")


# = Models = #
class ActiveBountyUpdateModel(BaseModel):
    bounty_ids: list[int]


@router.get("/claim")
async def claim_points(
    user: AuthenticatedUser = Depends(inject_authenticated_user),
    # = Static Game Data = #,
    static_bounties: StaticBounties = Depends(inject_static_bounties),
    # = Database Repositories = #
    bounties_repo: BountiesRepository = Depends(inject_bounties_repository),
    currency_repo: CurrencyRepository = Depends(inject_currency_repository),
):
    # Load data from mongo
    bounties_user_data: UserBountiesModel = await bounties_repo.get_user(user.id)

    # Calculate the unclaimed points
    unclaimed = calc_unclaimed_points(
        bounties_user_data, now := dt.datetime.utcnow(), s_bounties=static_bounties
    )

    # Update the claim time
    await bounties_repo.set_claim_time(user.id, claim_time=now)

    currencies = await currency_repo.update_one(
        user.id, {"$inc": {CurrencyRepoFields.BOUNTY_POINTS: unclaimed}}
    )

    return ServerResponse(
        {
            "claimTime": now,
            "currencyItems": currencies.response_dict(),
            "pointsClaimed": unclaimed,
        }
    )


@router.post("/setactive")
async def set_active_bounties(
    data: ActiveBountyUpdateModel,
    user: AuthenticatedUser = Depends(inject_authenticated_user),
    # = Static Game Data = #,
    static_bounties: StaticBounties = Depends(inject_static_bounties),
    # = Database Repositories = #
    bounties_repo: BountiesRepository = Depends(inject_bounties_repository),
):
    # Check that the user is attempting to activate an acceptable num of bounties
    check_num_active_bounties(data, s_bounties=static_bounties)

    # Load data from the mongo database
    bounty_data: UserBountiesModel = await bounties_repo.get_user(user.id)

    # Confirm that the user has the bounty unlocked
    check_unlocked_bounty(data, bounty_data)

    # Enable (or disable) the relevant bounties
    await bounties_repo.update_active_bounties(user.id, data.bounty_ids)

    # Refresh the user data from the database, ready to return it back to the user
    bounty_data: UserBountiesModel = await bounties_repo.get_user(user.id)

    return ServerResponse(bounty_data.response_dict())


# === Calculations === #


def calc_unclaimed_points(
    user_data: UserBountiesModel, now: dt.datetime, *, s_bounties: StaticBounties
) -> int:
    points = 0  # Total unclaimed points (ready to be claimed)

    # Interate over each active bounty available
    for bounty in user_data.active_bounties:
        s_bounty_data = utils.get(s_bounties.bounties, id=bounty.bounty_id)

        # Num. hours since the user has claimed this bounty
        total_hours = (now - user_data.last_claim_time).total_seconds() / 3_600

        # Clamp between 0 - max_unclaimed_hours
        hours_clamped = max(0, min(s_bounties.max_unclaimed_hours, total_hours))  # type: ignore

        # Calculate the income and increment the total
        points += math.floor(hours_clamped * s_bounty_data.income)

    return points


# === Checks === #


def check_num_active_bounties(
    data: ActiveBountyUpdateModel, s_bounties: StaticBounties
):

    # Check that the user is attempting to activate an allowed num of bounties
    if len(data.bounty_ids) > s_bounties.max_active_bounties:
        raise HTTPException(400, detail="Too many active bounties")

    return True


def check_unlocked_bounty(
    req_model: ActiveBountyUpdateModel, user_data: UserBountiesModel
):
    # Grab the bounty ids which the user has unlocked
    user_bounty_ids = [b.bounty_id for b in user_data.bounties]

    # User is attempting to use a bounty they do not have unlocked
    if not all(id_ in user_bounty_ids for id_ in req_model.bounty_ids):
        raise HTTPException(400, detail="Attempting to allow a locked bounty")

    return True
