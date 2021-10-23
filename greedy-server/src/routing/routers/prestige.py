from fastapi import Depends

from src.pymodels import BaseModel
from src.common import formulas
from src.common.enums import BonusType
from src.routing import ServerResponse, APIRouter

from src.resources.bounties import inject_static_bounties, StaticBounties
from src.resources.artefacts import inject_static_artefacts, StaticArtefact

from src.routing.dependencies.authenticated_user import AuthenticatedUser, inject_user

from src.mongo.repositories.bounties import BountiesRepository, inject_bounties_repository
from src.mongo.repositories.artefacts import ArtefactsRepository, ArtefactModel, inject_artefacts_repository
from src.mongo.repositories.currencies import (
    CurrenciesRepository,
    Fields as CurrencyRepoFields,
    inject_currencies_repository
)


router = APIRouter(prefix="/api")


# Models
class PrestigeData(BaseModel):
    prestige_stage: int = 5000


@router.post("/prestige")
async def prestige(
        data: PrestigeData,
        user: AuthenticatedUser = Depends(inject_user),

        # = Game Data = #
        s_bounties: StaticBounties = Depends(inject_static_bounties),
        s_artefacts: list[StaticArtefact] = Depends(inject_static_artefacts),

        bounties_repo: BountiesRepository = Depends(inject_bounties_repository),
        currency_repo: CurrenciesRepository = Depends(inject_currencies_repository),
        artefacts_repo: ArtefactsRepository = Depends(inject_artefacts_repository)
):
    user_arts = await artefacts_repo.get_all_artefacts(user.id)

    await process_prestige_points(
        user.id, data, artefacts=user_arts, currency_repo=currency_repo, s_artefacts=s_artefacts
    )
    await process_new_bounties(user.id, data, bounties_repo=bounties_repo, s_bounties=s_bounties)

    return ServerResponse({})


async def process_prestige_points(
        uid, req_data: PrestigeData, *, artefacts, currency_repo, s_artefacts: list[StaticArtefact]
):

    points = calc_prestige_points_at_stage(req_data.prestige_stage, artefacts, s_artefacts=s_artefacts)

    await currency_repo.update_one(uid, {
        "$inc": {
            CurrencyRepoFields.PRESTIGE_POINTS: points
        }
    })


async def process_new_bounties(
        uid, req_data: PrestigeData, *, bounties_repo: BountiesRepository, s_bounties: StaticBounties
):
    u_bounty_data = await bounties_repo.get_user(uid)

    u_bounty_ids = [b.bounty_id for b in u_bounty_data.bounties]

    for bounty in s_bounties.bounties:
        if bounty.id not in u_bounty_ids and req_data.prestige_stage >= bounty.stage:
            await bounties_repo.add_new_bounty(uid, bounty.id)


# == Calculations == #

def calc_prestige_points_at_stage(stage, artefacts: list[ArtefactModel], s_artefacts):
    base_points = formulas.base_points_at_stage(stage)

    multiplier = formulas.artefacts_bonus_product(BonusType.PRESTIGE_BONUS, artefacts, s_artefacts)

    return base_points * multiplier
