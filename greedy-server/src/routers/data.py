
from fastapi import APIRouter
from pydantic import BaseModel

from src import svrdata
from src import resources as res2
from src.common import mongo, resources
from src.routing import CustomRoute, ServerResponse

router = APIRouter(prefix="/api", route_class=CustomRoute)


class UserLoginData(BaseModel):
    device_id: str


@router.get("/gamedata")
def get_game_data():

    return {
        "artefactResources":    res2.get_artefacts().as_dict(),
        "mercResources":        resources.get_mercs(),
        "armouryResources":     res2.get_armoury().as_dict(),

        "bounties": resources.get("bounties"),
        "nextDailyReset": svrdata.next_daily_reset()
    }


@router.post("/login")
def player_login(data: UserLoginData):

    if (row := mongo.db["userLogins"].find_one({"deviceId": data.device_id})) is None:
        result = mongo.db["userLogins"].insert_one({"deviceId": data.device_id})

        uid = result.inserted_id

    else:
        uid = row["_id"]

    return ServerResponse(svrdata.get_player_data(uid))
