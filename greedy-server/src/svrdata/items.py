from pymongo import ReturnDocument

from src.common import mongo

BOUNTY_POINTS = "bountyPoints"
ARMOURY_POINTS = "ironIngots"
PRESTIGE_POINTS = "prestigePoints"


def find_one2(uid, *, post_process: bool = True) -> dict:
    result = mongo.db["userItems"].find_one({"userId": uid}) or dict()

    return _after_find(result) if post_process else result


def find_one(search, *, post_process: bool = True) -> dict:
    result = mongo.db["userItems"].find_one(search) or dict()

    return _after_find(result) if post_process else result


def update_one(search: dict, update: dict, *, upsert: bool = True) -> bool:
    result = mongo.db["userItems"].update_one(
        search, _before_update(search, update), upsert=upsert
    )

    return result.modified_count > 0


def find_and_update_one(search: dict, update: dict) -> dict:
    return _after_find(
        mongo.db["userItems"].find_one_and_update(
            search, _before_update(search, update), upsert=True, return_document=ReturnDocument.AFTER
        )
    )


def find_and_update_one2(uid, update: dict) -> dict:
    search = {"userId": uid}

    return _after_find(
        mongo.db["userItems"].find_one_and_update(
            search, _before_update(search, update), upsert=True, return_document=ReturnDocument.AFTER
        )
    )


def _after_find(result: dict):
    """ Perform datatype conversions (ex. string to BigInteger) """

    result[PRESTIGE_POINTS] = int(result.pop(PRESTIGE_POINTS, 0))

    return result


def _before_update(search: dict, update: dict):

    update = _move_inc_to_set(search, update, PRESTIGE_POINTS)

    return update


def _move_inc_to_set(search: dict, update: dict, key: str):

    if inc_amount := update["$inc"].get(key):
        has_amount = find_one(search).get(key, 0)

        update["$set"] = update.get("$set", dict())
        update["$set"] = {key: str(int(has_amount) + inc_amount)}

        update["$inc"].pop(key)

    if not update["$inc"]:
        update.pop("$inc")

    return update
