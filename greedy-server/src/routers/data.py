
from fastapi import APIRouter
from pydantic import BaseModel

from src import svrdata
from src.common import mongo, resources
from src.routing import CustomRoute, ServerResponse
from src.classes.gamedata import GameData

router = APIRouter(prefix="/api", route_class=CustomRoute)


class UserLoginData(BaseModel):
    device_id: str


@router.get("/gamedata")
def get_game_data():

    data = {
        "mercResources":    resources.get_mercs(),
        "armouryResources": resources.get("armouryitems"),

        "artefacts": resources.get("artefacts"),
        "bounties": resources.get("bounties"),
        "nextDailyReset": svrdata.next_daily_reset()
    }

    data.update(GameData.data)

    return data


@router.post("/login")
def player_login(data: UserLoginData):

    if (row := mongo.db["userLogins"].find_one({"deviceId": data.device_id})) is None:
        result = mongo.db["userLogins"].insert_one({"deviceId": data.device_id})

        uid = result.inserted_id

    else:
        uid = row["_id"]

    return ServerResponse(svrdata.get_player_data(uid))
