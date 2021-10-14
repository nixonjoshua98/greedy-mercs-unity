import math
import random

from fastapi import HTTPException, Depends

from src import utils
from src.models import UserIdentifier
from src.common import formulas
from src.checks import user_or_raise
from src.routing import ServerResponse, APIRouter
from src.routing.common.checks import check_greater_than, check_is_not_none

from src.resources.artefacts import inject_static_artefacts, StaticArtefact

from src.mongo.repositories.artefacts import (
    ArtefactsRepository, ArtefactModel, Fields as ArtefactsRepoFields, artefacts_repository
)

from src.mongo.repositories.currencies import (
    CurrenciesRepository, Fields as CurrencyRepoFields, currencies_repository
)

router = APIRouter(prefix="/api/artefact")


# == Models == #

class ArtefactUpgradeModel(UserIdentifier):
    artefact_id: int
    upgrade_levels: int


# == Endpoints == #

@router.post("/upgrade")
async def upgrade(
        data: ArtefactUpgradeModel,

        # = Static Game Data = #
        static_artefacts=Depends(inject_static_artefacts),

        # = Database Repositories = #
        artefacts_repo: ArtefactsRepository = Depends(artefacts_repository),
        currency_repo: CurrenciesRepository = Depends(currencies_repository)
):
    uid = await user_or_raise(data)

    # Pull the artefact in question
    s_artefact = utils.get(static_artefacts, id=data.artefact_id)

    # Check that the request artefact actually exists
    check_is_not_none(s_artefact, error="Artefact is not valid")

    # Load the related artefact
    user_art = await artefacts_repo.get_one_artefact(uid, data.artefact_id)

    # Verify that the user has the artefact unlocked
    check_is_not_none(user_art, error="Artefact is not unlocked")

    # Check that upgrading this artefact will not exceed the max level
    check_artefact_within_max_level(user_art, s_artefact, data.upgrade_levels)

    # Calculate the upgrade cost for the artefact
    upgrade_cost = calc_upgrade_cost(user_art, s_artefact, data.upgrade_levels)

    # Fetch the currency to upgrade the item
    currencies = await currency_repo.get_user(uid)

    # Check that the user can afford the upgrade cost
    check_greater_than(currencies.prestige_points, upgrade_cost, error="Cannot afford to upgrade artefact")

    # Update the database
    currencies = await currency_repo.update_one(uid, {
        "$inc": {
            CurrencyRepoFields.PRESTIGE_POINTS: -upgrade_cost
        }
    })

    # Update the artefact
    artefact = await artefacts_repo.update_artefact(uid, data.artefact_id, {
        "$inc": {
            ArtefactsRepoFields.LEVEL: data.upgrade_levels
        }
    })

    return ServerResponse({"currencyItems": currencies.response_dict(), "updatedArtefact": artefact.response_dict()})


@router.post("/unlock")
async def unlock(
        data: UserIdentifier,

        # = Static Game Data = #
        static_artefacts=Depends(inject_static_artefacts),

        # = Database Repositories = #
        artefacts_repo: ArtefactsRepository = Depends(artefacts_repository),
        currency_repo: CurrenciesRepository = Depends(currencies_repository)
):
    uid = await user_or_raise(data)

    # Fetch all user artefacts
    user_arts = await artefacts_repo.get_all_artefacts(uid)

    # Verify that the user still has an artefact available to unlock
    check_not_unlocked_all_artefacts(user_arts, static_artefacts)

    # Calculate the artefact cost
    unlock_cost = calc_unlock_cost(user_arts)

    # Fetch the currencies from the database
    currencies = await currency_repo.get_user(uid)

    # Verify that the user can afford the unlock cost
    check_greater_than(currencies.prestige_points, unlock_cost, error="Cannot afford unlock cost")

    # Get the new artefact id
    new_art_id = get_new_artefact(user_arts, static_artefacts)

    # Add the new artefact
    new_artefact = await artefacts_repo.add_new_artefact(uid, new_art_id)

    # Update the database
    currencies = await currency_repo.update_one(uid, {
        "$inc": {
            CurrencyRepoFields.PRESTIGE_POINTS: -unlock_cost
        }
    })

    return ServerResponse({"currencyItems": currencies.response_dict(), "newArtefact": new_artefact.response_dict()})


# == Calculations == #

def calc_unlock_cost(artefacts: list[ArtefactModel]):
    return math.floor(max(1, num_arts := len(artefacts) - 2) * math.pow(1.35, num_arts))


def get_new_artefact(artefacts: list[ArtefactModel], s_artefacts: list[StaticArtefact]):
    ids: list[int] = [art.id for art in s_artefacts]
    u_arts_ids: list[int] = [art.artefact_id for art in artefacts]

    return random.choice(list(set(ids) - set(u_arts_ids)))


def calc_upgrade_cost(u_art: ArtefactModel, s_art: StaticArtefact, levels: int) -> int:
    return formulas.artefact_upgrade_cost(s_art.cost_coeff, s_art.cost_expo, u_art.level, levels)


# == Checks == #

def check_not_unlocked_all_artefacts(artefacts: list[ArtefactModel], static_artefacts: list[StaticArtefact]):

    if len(artefacts) >= len(static_artefacts):
        raise HTTPException(400, detail="Max number of artefacts unlocked")


def check_artefact_within_max_level(artefact: ArtefactModel, static_art: StaticArtefact, levels: int):
    """ Confirm that levelling the artefact will not exceed the artefact max level """

    if (artefact.level + levels) > static_art.max_level:
        raise HTTPException(400, detail="Level will exceed max level")
