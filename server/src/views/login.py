from flask import jsonify, request

from src import serverutils
from src.server import mongo
from src.data.enums import HeroID
from src.data.servercodes import ServerCodes


def login():
    data = request.get_json()

    if (device_id := data.get("deviceId")) is None:
        return jsonify({"message": " "}), ServerCodes.BAD_INPUT

    # - New user login
    if (user_id := serverutils.device_to_id(mongo, data["deviceId"])) is None:
        result = mongo.db.users.update_one({"deviceId": device_id}, {"$set": {"deviceId": device_id}}, upsert=True)

        user_id = result.upserted_id

        # Insert default heroes (temporary)
        mongo.db.heroes.insert_many(
            [
                {"userId": user_id, "heroId": HeroID.WRAITH_LIGHTNING},
                {"userId": user_id, "heroId": HeroID.FALLEN_ANGEL},
                {"userId": user_id, "heroId": HeroID.GOLEM_STONE},
                {"userId": user_id, "heroId": HeroID.SATYR_FIRE},
            ]
        )

    data = {
        "heroes": serverutils.get_user_heroes(mongo, user_id)
    }

    return jsonify(data)
