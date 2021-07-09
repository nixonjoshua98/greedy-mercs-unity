
from fastapi import APIRouter

from src import dbutils
from src.common import resources

from src.classes.gamedata import GameData

from src.common import mongo

from fastapi_src.basemodels import UserLoginModel
from fastapi_src.routing import CustomRoute, ServerResponse

router = APIRouter(prefix="/api", route_class=CustomRoute)


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
def player_login(data: UserLoginModel):

    # - New login detected
    if (row := mongo.db.userLogins.find_one({"deviceId": data.device_id})) is None:
        result = mongo.db.userLogins.insert_one({"deviceId": data.device_id})

        uid = result.inserted_id

    else:
        uid = row["_id"]

    return ServerResponse(dbutils.get_player_data(uid))
