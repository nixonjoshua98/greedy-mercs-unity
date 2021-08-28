from fastapi import APIRouter

from src import resources as res2
from src.models import UserLoginDataModel
from src.common import resources
from src.routing import ServerRoute, ServerResponse

from src.dataloader import MongoController, DataLoader

router = APIRouter(prefix="/api", route_class=ServerRoute)


@router.get("/gamedata")
def get_game_data():
    loader = DataLoader()

    svr_state = loader.get_server_state()

    return ServerResponse(
        {
            "artefactResources": res2.get_artefacts().as_dict(),
            "mercResources": resources.get_mercs(),
            "armouryResources": res2.get_armoury().as_dict(),

            "bounties": res2.get_bounty_data(as_dict=True),
            "nextDailyReset": svr_state.next_daily_reset
        }
    )


@router.post("/login")
async def player_login(data: UserLoginDataModel):

    with MongoController() as mongo:
        user = await mongo.users.get_user(data.device_id)

        if user is None:
            uid = await mongo.users.insert_new_user(
                device=data.device_id
            )

        else:
            uid = user["_id"]

        u_data = await mongo.get_user_data(uid)

    return ServerResponse(u_data)
