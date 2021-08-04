from bson import ObjectId
from typing import Union
from pymongo import ReturnDocument

from src.common.enums import ItemKeys

from src.database.mongo import get_collection

__collection_name__ = "userItems"


def get_user_items(uid: Union[str, ObjectId], *, post_process: bool = True) -> dict:
    """ Retrieve all user items

    Args:
        uid: User
        post_process (bool): Choose to process the result (ex. convert string number to integer)

    Returns:
        dict: User items
    """
    result = get_collection(__collection_name__).find_one({"userId": uid}) or dict()

    return _after_find(result) if post_process else result


def get_user_item(uid: Union[str, ObjectId], key: str) -> int:
    """ Retrieve a single item

    Args:
        uid: User
        key (str): Key which we want to get

    Returns:
        dict: Single user item
    """
    return get_user_items(uid).get(key, 0)


def update_and_get_user_items(uid: Union[str, ObjectId], update: dict) -> dict:
    """ Update a single document, with the uid, and return the document

    Args:
        uid: User
        update (dict): The update part of the Mongo query

    Returns:
        dict: User's items AFTER the query was executed
    """
    return _after_find(
        get_collection(__collection_name__).find_one_and_update(
            {"userId": uid}, _before_update(uid, update), upsert=True, return_document=ReturnDocument.AFTER
        )
    )


def update_user_items(uid: Union[str, ObjectId], update: dict, *, upsert: bool = True) -> bool:
    """ Update a single document, with the uid

    Args:
        uid: User
        update (dict): The update part of the Mongo query
        upsert (bool): Choose to insert a new row, if an existing one does not exist
    Returns:
        bool: Whether a document was modified
    """
    result = get_collection(__collection_name__).update_one({"userId": uid}, _before_update(uid, update), upsert=upsert)

    return result.modified_count > 0


def _before_update(uid, update: dict):
    update = _move_inc_to_set(uid, update, ItemKeys.PRESTIGE_POINTS)

    return update


def _after_find(result: dict):
    """ Perform datatype conversions (ex. string to BigInteger) """

    result[ItemKeys.PRESTIGE_POINTS] = int(result.pop(ItemKeys.PRESTIGE_POINTS, 0))

    return result


def _move_inc_to_set(uid, update: dict, key: str):
    """
    This will convert a $inc statement to a $set statement. This is used on fields which are stored as
    a string since they exceed Mongo max integer. This conversion requires an addition database query
    and is not an ideal work around. Note: Look into storing a mantissa/exponent
    """
    if inc_amount := update["$inc"].get(key):
        has_amount = get_user_item(uid, key)

        update["$set"] = update.get("$set", dict())
        update["$set"][key] = str(has_amount + inc_amount)

        update["$inc"].pop(key)

    if not update["$inc"]:
        update.pop("$inc")

    return update
