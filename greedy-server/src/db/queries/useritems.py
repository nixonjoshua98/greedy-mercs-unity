from bson import ObjectId
from typing import Union

from pymongo import ReturnDocument

from src.common.enums import ItemKeys


async def get_items(client, uid: Union[str, ObjectId], *, post_process: bool = True) -> dict:
    result = (await client.get_default_database()["userItems"].find_one({"userId": uid})) or dict()

    return await _after_find(result) if post_process else result


async def get_item(client, uid: Union[str, ObjectId], key: str, *, post_process=True) -> int:
    return (await get_items(client, uid, post_process=post_process)).get(key, 0)


async def update_and_get_items(client, uid: Union[str, ObjectId], update: dict) -> dict:
    return await _after_find(
        await client.get_default_database()["userItems"].find_one_and_update(
            {"userId": uid}, await _before_update(client, uid, update),
            upsert=True, return_document=ReturnDocument.AFTER
        )
    )


async def update_items(client, uid: Union[str, ObjectId], update: dict, *, upsert: bool = True) -> bool:
    return await client.get_default_database()["userItems"].update_one(
        {"userId": uid}, await _before_update(client, uid, update), upsert=upsert
    ).modified_count > 0

# = = = = = = #


async def _before_update(client, uid: Union[str, ObjectId], update: dict):
    update = await _move_inc_to_set(client, uid, update, ItemKeys.PRESTIGE_POINTS)

    return update


async def _after_find(result: dict):
    """ Perform datatype conversions (ex. string to BigInteger) """

    result[ItemKeys.PRESTIGE_POINTS] = int(result.pop(ItemKeys.PRESTIGE_POINTS, 0))

    return result


async def _move_inc_to_set(client, uid: Union[str, ObjectId], update: dict, key: str):
    """
    This will convert a $inc statement to a $set statement. This is used on fields which are stored as
    a string since they exceed Mongo max integer. This conversion requires an addition database query
    and is not an ideal work around. Note: Look into storing a mantissa/exponent
    """
    if inc_amount := update["$inc"].get(key):
        amount = await get_item(client, uid, key)

        update["$set"] = update.get("$set", dict())
        update["$set"][key] = str(amount + inc_amount)

        update["$inc"].pop(key)

    if not update["$inc"]:
        update.pop("$inc")

    return update
