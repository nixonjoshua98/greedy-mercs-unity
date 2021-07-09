
from src.common import mongo

from fastapi import HTTPException

from src.basemodels import UserIdentifier


def user_or_raise(user: UserIdentifier):
    result = mongo.db["userLogins"].find_one({"deviceId": user.device_id})

    if result is None:
        raise HTTPException(400)

    return result["_id"]
