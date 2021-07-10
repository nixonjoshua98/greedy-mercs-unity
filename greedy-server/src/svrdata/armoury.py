
from src.common import mongo


def get_armoury(uid, *, iid=None):

    if iid is None:
        return list(mongo.db["userArmouryItems"].find({"userId": uid}))

    return mongo.db["userArmouryItems"].find_one({"userId": uid, "itemId": iid})


def update_one_item(uid, iid, inc, *, upsert: bool = True) -> bool:
    """ Update a single armoury item document

    Update a single document which shares the same owner and item identifier
    and return whether a document was modified.

    Args:
        uid (bson.ObjectId): Mongo User ID
        iid (int/str): Item Id
        inc (dict): The '$inc' section of the mongo query
        upsert (bool): Whether to upsert the document

    Returns:
        bool: Whether a document was modified
    """

    result = mongo.db["userArmouryItems"].update_one({"userId": uid, "itemId": iid}, {"$inc": inc}, upsert=upsert)

    return result.modified_count > 0
