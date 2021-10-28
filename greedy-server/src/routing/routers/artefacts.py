import math
import random

from fastapi import Depends, HTTPException

from src import utils
from src.common import formulas
from src.mongo.repositories.artefacts import ArtefactModel, ArtefactsRepository
from src.mongo.repositories.artefacts import Fields as ArtefactsRepoFields
from src.mongo.repositories.artefacts import inject_artefacts_repository
from src.mongo.repositories.currency import CurrencyRepository
from src.mongo.repositories.currency import Fields as CurrencyRepoFields
from src.mongo.repositories.currency import inject_currency_repository
from src.pymodels import BaseModel
from src.resources.artefacts import StaticArtefact, inject_static_artefacts
from src.routing import APIRouter, ServerResponse
from src.routing.common.checks import gt, is_not_none
from src.routing.dependencies.authenticated_user import AuthenticatedUser, inject_user

router = APIRouter(prefix="/api/artefact")


# == Models == #


class ArtefactUpgradeModel(BaseModel):
    artefact_id: int
    upgrade_levels: int


# == Endpoints == #


@router.post("/upgrade")
async def upgrade(
    data: ArtefactUpgradeModel,
    user: AuthenticatedUser = Depends(inject_user),
    # = Static Game Data = #
    static_artefacts=Depends(inject_static_artefacts),
    # = Database Repositories = #
    artefacts_repo: ArtefactsRepository = Depends(inject_artefacts_repository),
    currency_repo: CurrencyRepository = Depends(inject_currency_repository),
):
    # Pull the artefact in question
    s_artefact = utils.get(static_artefacts, id=data.artefact_id)

    # Check that the request artefact actually exists
    is_not_none(s_artefact, error="Artefact is not valid")

    # Load the related artefact
    user_art = await artefacts_repo.get_one_artefact(user.id, data.artefact_id)

    # Verify that the user has the artefact unlocked
    is_not_none(user_art, error="Artefact is not unlocked")

    # Check that upgrading this artefact will not exceed the max level
    check_artefact_within_max_level(user_art, s_artefact, data.upgrade_levels)

    # Calculate the upgrade cost for the artefact
    upgrade_cost = formulas.artefact_upgrade_cost(
        s_artefact, user_art.level, data.upgrade_levels
    )

    # Fetch the currency to upgrade the item
    currencies = await currency_repo.get_user(user.id)

    # Check that the user can afford the upgrade cost
    gt(
        currencies.prestige_points,
        upgrade_cost,
        error="Cannot afford to upgrade artefact",
    )

    # Update the database
    currencies = await currency_repo.update_one(
        user.id, {"$inc": {CurrencyRepoFields.PRESTIGE_POINTS: -upgrade_cost}}
    )

    # Update the artefact
    artefact = await artefacts_repo.update_artefact(
        user.id,
        data.artefact_id,
        {"$inc": {ArtefactsRepoFields.LEVEL: data.upgrade_levels}},
    )

    return ServerResponse(
        {
            "currencyItems": currencies.response_dict(),
            "updatedArtefact": artefact.response_dict(),
            "upgradeCost": upgrade_cost,
        }
    )


@router.get("/unlock")
async def unlock(
    user: AuthenticatedUser = Depends(inject_user),
    # = Static Game Data = #
    static_artefacts=Depends(inject_static_artefacts),
    # = Database Repositories = #
    artefacts_repo: ArtefactsRepository = Depends(inject_artefacts_repository),
    currency_repo: CurrencyRepository = Depends(inject_currency_repository),
):
    # Fetch all user artefacts
    user_arts = await artefacts_repo.get_all_artefacts(user.id)

    # Verify that the user still has an artefact available to unlock
    check_not_unlocked_all_artefacts(user_arts, static_artefacts)

    # Calculate the artefact cost
    unlock_cost = calc_unlock_cost(user_arts)

    # Fetch the currencies from the database
    currencies = await currency_repo.get_user(user.id)

    # Verify that the user can afford the unlock cost
    gt(currencies.prestige_points, unlock_cost, error="Cannot afford unlock cost")

    # Get the new artefact id
    new_art_id = get_new_artefact(user_arts, static_artefacts)

    # Add the new artefact
    new_artefact = await artefacts_repo.add_new_artefact(user.id, new_art_id)

    # Update the database
    currencies = await currency_repo.update_one(
        user.id, {"$inc": {CurrencyRepoFields.PRESTIGE_POINTS: -unlock_cost}}
    )

    return ServerResponse(
        {
            "currencyItems": currencies.response_dict(),
            "newArtefact": new_artefact.response_dict(),
        }
    )


# == Calculations == #


def calc_unlock_cost(artefacts: list[ArtefactModel]):
    return math.floor(max(1, num_arts := len(artefacts) - 2) * math.pow(1.35, num_arts))


def get_new_artefact(artefacts: list[ArtefactModel], s_artefacts: list[StaticArtefact]):
    ids: list[int] = [art.id for art in s_artefacts]
    u_arts_ids: list[int] = [art.artefact_id for art in artefacts]

    return random.choice(list(set(ids) - set(u_arts_ids)))


# == Checks == #


def check_not_unlocked_all_artefacts(
    artefacts: list[ArtefactModel], static_artefacts: list[StaticArtefact]
):

    if len(artefacts) >= len(static_artefacts):
        raise HTTPException(400, detail="Max number of artefacts unlocked")


def check_artefact_within_max_level(
    artefact: ArtefactModel, static_art: StaticArtefact, levels: int
):
    """Confirm that levelling the artefact will not exceed the artefact max level"""

    if (artefact.level + levels) > static_art.max_level:
        raise HTTPException(400, detail="Level will exceed max level")
