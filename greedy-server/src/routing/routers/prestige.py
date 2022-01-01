from fastapi import Depends

from src.common import formulas
from src.common.enums import BonusType
from src.mongo.repositories.artefacts import (ArtefactModel,
                                              ArtefactsRepository,
                                              artefacts_repository)
from src.mongo.repositories.bounties import (BountiesRepository,
                                             bounties_repository)
from src.mongo.repositories.currency import CurrencyRepository
from src.mongo.repositories.currency import Fields as CurrencyRepoFields
from src.mongo.repositories.currency import currency_repository
from src.pymodels import BaseModel
from src.request_context import (AuthenticatedRequestContext,
                                 authenticated_context)
from src.resources.artefacts import StaticArtefact, static_artefacts
from src.resources.bounties import StaticBounties, inject_static_bounties
from src.routing import APIRouter, ServerResponse

from ..handlers.data import (GetStaticData, GetUserDataHandler,
                             StaticDataResponse, UserDataResponse)

router = APIRouter()


class PrestigeData(BaseModel):
    prestige_stage: int = 5000


@router.post("/")
async def prestige(
    data: PrestigeData,
    ctx: AuthenticatedRequestContext = Depends(authenticated_context),
    s_bounties: StaticBounties = Depends(inject_static_bounties),
    s_artefacts: list[StaticArtefact] = Depends(static_artefacts),
    bounties_repo: BountiesRepository = Depends(bounties_repository),
    currency_repo: CurrencyRepository = Depends(currency_repository),
    artefacts_repo: ArtefactsRepository = Depends(artefacts_repository),
    user_data_handler: GetUserDataHandler = Depends(),
    static_data_handler: GetStaticData = Depends(),
):
    user_arts = await artefacts_repo.get_all_artefacts(ctx.user_id)

    await process_prestige_points(
        ctx.user_id,
        data,
        artefacts=user_arts,
        currency_repo=currency_repo,
        s_artefacts=s_artefacts,
    )
    await process_new_bounties(
        ctx.user_id, data, bounties_repo=bounties_repo, s_bounties=s_bounties
    )

    u_data_resp: UserDataResponse = await user_data_handler.handle(ctx)
    s_data_resp: StaticDataResponse = await static_data_handler.handle()

    return ServerResponse({
        "userData": u_data_resp.data,
        "staticData": s_data_resp.data
    })


async def process_prestige_points(
    uid,
    req_data: PrestigeData,
    *,
    artefacts,
    currency_repo,
    s_artefacts: list[StaticArtefact]
):

    points = calc_prestige_points_at_stage(
        req_data.prestige_stage, artefacts, s_artefacts=s_artefacts
    )

    await currency_repo.update_one(
        uid, {"$inc": {CurrencyRepoFields.PRESTIGE_POINTS: points}}
    )


async def process_new_bounties(
    uid,
    req_data: PrestigeData,
    *,
    bounties_repo: BountiesRepository,
    s_bounties: StaticBounties
):
    u_bounty_data = await bounties_repo.get_user_bounties(uid)

    u_bounty_ids = [b.bounty_id for b in u_bounty_data.bounties]

    for bounty in s_bounties.bounties:
        if bounty.id not in u_bounty_ids and req_data.prestige_stage >= bounty.stage:
            await bounties_repo.add_new_bounty(uid, bounty.id)


def calc_prestige_points_at_stage(stage, artefacts: list[ArtefactModel], s_artefacts):
    base_points = formulas.base_points_at_stage(stage)

    multiplier = formulas.artefacts_bonus_product(
        BonusType.MULTIPLY_PRESTIGE_BONUS, artefacts, s_artefacts
    )

    return base_points * multiplier
