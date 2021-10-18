import math

from fastapi import Depends

from src.common import formulas
from src.common.enums import BonusType
from src.checks import user_or_raise
from src.routing import ServerResponse, APIRouter
from src.models import UserIdentifier

from src import resources

from src.mongo.repositories.bounties import BountiesRepository, inject_bounties_repository
from src.mongo.repositories.artefacts import ArtefactsRepository, ArtefactModel, inject_artefacts_repository
from src.mongo.repositories.currencies import CurrenciesRepository, Fields as CurrencyRepoFields, inject_currencies_repository


router = APIRouter(prefix="/api")


# Models
class PrestigeData(UserIdentifier):
    prestige_stage: int = 500


@router.post("/prestige")
async def prestige(
        data: PrestigeData,
        bounties_repo: BountiesRepository = Depends(inject_bounties_repository),
        currency_repo: CurrenciesRepository = Depends(inject_currencies_repository),
        artefacts_repo: ArtefactsRepository = Depends(inject_artefacts_repository)
):
    uid = await user_or_raise(data)

    user_arts = await artefacts_repo.get_all_artefacts(uid)

    await process_prestige_points(uid, data, artefacts=user_arts, currency_repo=currency_repo)
    await process_new_bounties(uid, data, bounties_repo=bounties_repo)

    return ServerResponse({})


async def process_prestige_points(uid, req_data: PrestigeData, *, artefacts, currency_repo):

    points = calc_prestige_points_at_stage(req_data.prestige_stage, artefacts)

    await currency_repo.update_one(uid, {
        "$inc": {
            CurrencyRepoFields.PRESTIGE_POINTS: points
        }
    })


async def process_new_bounties(uid, req_data: PrestigeData, *, bounties_repo: BountiesRepository):
    bounty_res = resources.get_bounty_data()

    u_bounty_data = await bounties_repo.get_user(uid)

    u_bounty_ids = [b.bounty_id for b in u_bounty_data.bounties]

    for key, bounty in bounty_res.bounties.items():
        if key not in u_bounty_ids and req_data.prestige_stage >= bounty.stage:
            await bounties_repo.add_new_bounty(uid, key)


# == Calculations == #

def calc_prestige_points_at_stage(stage, artefacts: list[ArtefactModel]):
    multiplier = calc_prestige_points_artefact_multiplier(artefacts)

    return math.ceil(math.pow(math.ceil((max(stage, 75) - 75) / 10.0), 2.2) * multiplier)


def calc_prestige_points_artefact_multiplier(artefacts: list[ArtefactModel]):
    from src.common import resources as oldres

    bonus = 1

    static_arts = oldres.get("artefacts")

    for art in artefacts:
        s_art = static_arts[art.artefact_id]

        if s_art["bonusType"] == BonusType.PRESTIGE_BONUS:
            bonus *= formulas.artefact_effect(s_art, art.level)

    return bonus
