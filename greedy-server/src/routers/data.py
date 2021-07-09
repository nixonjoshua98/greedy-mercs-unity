
from fastapi import APIRouter
from pydantic import BaseModel

from src import dbutils
from src.common import mongo, resources
from src.routing import CustomRoute, ServerResponse
from src.classes.gamedata import GameData

router = APIRouter(prefix="/api", route_class=CustomRoute)


class UserLoginData(BaseModel):
    device_id: str


@router.get("/gamedata")
def get_game_data():

    data = {
        "mercData": resources.get_mercs(),
        "artefacts": resources.get("artefacts"),
        "bounties": resources.get("bounties"),
        "armouryItems": resources.get("armouryitems"),
        "nextDailyReset": dbutils.next_daily_reset()}

    data.update(GameData.data)

    return data


@router.post("/login")
def player_login(data: UserLoginData):

    if (row := mongo.db["userLogins"].find_one({"deviceId": data.device_id})) is None:
        result = mongo.db["userLogins"].insert_one({"deviceId": data.device_id})

        uid = result.inserted_id

    else:
        uid = row["_id"]

    return ServerResponse(dbutils.get_player_data(uid))
