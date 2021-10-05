
from fastapi import APIRouter, Depends

from src.common import formulas
from src.common.enums import ItemKey
from src.checks import user_or_raise
from src.routing import ServerRoute, ServerResponse
from src.models import UserIdentifier

from src.dataloader import DataLoader
from src import resources

from src.mongo.repositories.bounties import (
    BountiesRepository,
    bounties_repository
)


router = APIRouter(prefix="/api", route_class=ServerRoute)


# Models
class PrestigeData(UserIdentifier):
    prestige_stage: int


@router.post("/prestige")
async def prestige(
        data: PrestigeData,
        bounties_repo: BountiesRepository = Depends(bounties_repository)
):
    uid = await user_or_raise(data)

    loader = DataLoader()

    await process_prestige_points(uid, data, loader=loader)
    await process_new_bounties(uid, data, bounties_repo=bounties_repo)

    u_data = await loader.get_user_data(uid)

    return ServerResponse({"userData": u_data})


async def process_prestige_points(uid, req_data: PrestigeData, *, loader: DataLoader):

    u_artefacts = await loader.artefacts.get_all_artefacts(uid)

    points_gained = formulas.stage_prestige_points(req_data.prestige_stage, u_artefacts)

    await loader.items.update_items(uid, {"$inc": {ItemKey.PRESTIGE_POINTS: points_gained}})


async def process_new_bounties(uid, req_data: PrestigeData, *, bounties_repo: BountiesRepository):
    bounty_res = resources.get_bounty_data()

    u_bounty_data = await bounties_repo.get_user(uid)

    u_bounty_ids = [b.bounty_id for b in u_bounty_data.bounties]

    u_bounty_ids = [b["bountyId"] for b in u_bounties]

    for key, bounty in bounty_res.bounties.items():
        if key not in u_bounty_ids and req_data.prestige_stage >= bounty.stage:
            await bounties_repo.add_new_bounty(uid, key)
