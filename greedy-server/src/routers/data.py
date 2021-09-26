from fastapi import APIRouter

import datetime as dt

from src.models import UserLoginDataModel
from src.common import resources
from src.routing import ServerRoute, ServerResponse
from src.classes import ServerState
from src.dataloader import DataLoader
from src import resources as res2

router = APIRouter(prefix="/api", route_class=ServerRoute)


@router.get("/gamedata")
def get_game_data():
    svr_state = ServerState()

    return ServerResponse(
        {
            "artefacts_gameData": res2.get_artefacts_data(as_dict=True),
            "mercs_gameData": resources.get_mercs(),
            "armoury_gameData": res2.get_armoury_resources(as_dict=True),

            "bounties_gameData": res2.get_bounty_data(as_dict=True),
            "nextDailyReset": svr_state.next_daily_reset
        }
    )


@router.post("/login")
async def player_login(data: UserLoginDataModel):
    mongo = DataLoader()

    user = await mongo.users.get_user(data.device_id)

    if user is None:
        uid = await mongo.users.insert_new_user({
            "deviceId": data.device_id,
            "accountCreationTime": dt.datetime.utcnow()
        })

    else:
        uid = user["_id"]

    u_data = await mongo.get_user_data(uid)

    return ServerResponse(u_data)
