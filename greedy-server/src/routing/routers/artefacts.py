import math
import random

from fastapi import HTTPException, Depends

from src.resources.artefacts import ArtefactResourceData
from src.checks import user_or_raise
from src.common import formulas
from src.routing import ServerResponse, APIRouter
from src.models import UserIdentifier
from src import resources

from src.routing.common.checks import (
    check_can_afford,
    check_is_not_none
)

from src.mongo.repositories.artefacts import (
    ArtefactsRepository,
    ArtefactModel,
    Fields as ArtefactsRepoFields,
    artefacts_repository
)

from src.mongo.repositories.currencies import (
    CurrenciesRepository,
    Fields as CurrencyRepoFields,
    currencies_repository
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
        artefacts_repo: ArtefactsRepository = Depends(artefacts_repository),
        currency_repo: CurrenciesRepository = Depends(currencies_repository)
):
    uid = await user_or_raise(data)

    # Check that the request artefact actually exists
    check_valid_artefact(data.artefact_id)

    # Load the related artefact
    user_art = await artefacts_repo.get_artefact(uid, data.artefact_id)

    # Verify that the user has the artefact unlocked
    check_is_not_none(user_art, error="Artefact is not unlocked")

    # Check that upgrading this artefact will not exceed the max level
    check_artefact_within_max_level(user_art, data.upgrade_levels)

    # Calculate the upgrade cost for the artefact
    upgrade_cost = calc_upgrade_cost(user_art, data.upgrade_levels)

    # Fetch the currency to upgrade the item
    currencies = await currency_repo.get_user(uid)

    # Check that the user can afford the upgrade cost
    check_can_afford(currencies.prestige_points, upgrade_cost, error="Cannot afford to upgrade artefact")

    # Update the database
    currencies = await currency_repo.update_one(uid, {
        "$inc": {
            CurrencyRepoFields.PRESTIGE_POINTS: -upgrade_cost
        }
    })

    # Update the artefact
    updated_artefact = await artefacts_repo.update_artefact(uid, data.artefact_id, {
        "$inc": {
            ArtefactsRepoFields.LEVEL: data.upgrade_levels
        }
    })

    return ServerResponse(
        {"userCurrencies": currencies.response_dict(), "updatedArtefact": updated_artefact.response_dict()}
    )


@router.post("/unlock")
async def unlock(
        data: UserIdentifier,
        artefacts_repo: ArtefactsRepository = Depends(artefacts_repository),
        currency_repo: CurrenciesRepository = Depends(currencies_repository)
):
    uid = await user_or_raise(data)

    # Fetch all user artefacts
    u_artefacts = await artefacts_repo.get_all_artefacts(uid)

    currencies = await currency_repo.get_user(uid)

    # Verify that the user still has an artefact available to unlock
    check_not_unlocked_all_artefacts(u_artefacts)

    # Calculate the artefact cost
    unlock_cost = calc_unlock_cost(u_artefacts)

    # Verify that the user can afford the unlock cost
    check_can_afford(currencies.prestige_points, unlock_cost, error="Cannot afford unlock cost")

    # Get the new artefact id
    new_art_id = get_new_artefact(u_artefacts)

    # Add the new artefact
    new_artefact = await artefacts_repo.add_new_artefact(uid, new_art_id)

    # Update the database
    currencies = await currency_repo.update_one(uid, {
        "$inc": {
            CurrencyRepoFields.PRESTIGE_POINTS: -unlock_cost
        }
    })

    return ServerResponse({"userCurrencies": currencies.response_dict(), "newArtefact": new_artefact.response_dict()})


# == Calculations == #

def calc_unlock_cost(artefacts: list[ArtefactModel]):
    return math.floor(max(1, num_arts := len(artefacts) - 2) * math.pow(1.35, num_arts))


def get_new_artefact(artefacts: list[ArtefactModel]):
    s_arts = resources.get_artefacts_data().artefacts

    return random.choice(list(set(list(s_arts.keys())) - set(list([art.id for art in artefacts]))))


def calc_upgrade_cost(artefact: ArtefactModel, levels: int) -> int:
    static_art = resources.get_artefacts_data().artefacts[artefact.artefact_id]

    return formulas.upgrade_artefact_cost(static_art.cost_coeff, static_art.cost_expo, artefact.level, levels)


# == Checks == #

def check_not_unlocked_all_artefacts(artefacts: list[ArtefactModel]):
    static_artefacts = resources.get_artefacts_data().artefacts

    if len(artefacts) >= len(static_artefacts):
        raise HTTPException(400, detail="Max number of artefacts unlocked")


def check_valid_artefact(artefact_id: int):
    s_arts = resources.get_artefacts_data().artefacts

    if artefact_id not in s_arts.keys():
        raise HTTPException(400, detail="Artefact is not valid")


def check_artefact_within_max_level(artefact: ArtefactModel, levels: int):
    static_artefact: ArtefactResourceData = resources.get_artefacts_data().artefacts[artefact.artefact_id]

    if (artefact.level + levels) > static_artefact.max_level:
        raise HTTPException(400, detail="Level will exceed max level")
