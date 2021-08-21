
from fastapi import APIRouter

from src.common import formulas
from src.common.enums import ItemKey
from src.checks import user_or_raise
from src.routing import ServerRoute, ServerResponse
from src.models import UserIdentifier

from src import resources, dataloader
from src.dataloader import MongoController

router = APIRouter(prefix="/api", route_class=ServerRoute)


# Models
class PrestigeData(UserIdentifier):
    prestige_stage: int


@router.post("/prestige")
async def prestige(data: PrestigeData):
    uid = user_or_raise(data)

    with MongoController() as mongo:

        loader = dataloader.get_loader()

        await process_prestige_points(uid, data)
        await process_new_bounties(uid, data)

    return ServerResponse({"userData": await loader.get_user_data(uid)})


async def process_prestige_points(uid, req_data: PrestigeData):
    loader = dataloader.get_loader()

    u_arts = await loader.artefacts.get_all(uid)

    points_gained = formulas.stage_prestige_points(req_data.prestige_stage, u_arts)

    await loader.user_items.update_items(uid, {"$inc": {ItemKey.PRESTIGE_POINTS: points_gained}})


async def process_new_bounties(uid, req_data: PrestigeData):
    loader = dataloader.get_loader()

    bounty_res = resources.get_bounty_data()

    u_bounties = await loader.bounties.get_user_bounties(uid)

    for key, bounty in bounty_res.bounties.items():
        if key not in u_bounties and req_data.prestige_stage >= bounty.stage:
            await loader.bounties.insert_user_bounty(uid, key)
