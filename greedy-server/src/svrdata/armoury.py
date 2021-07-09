
from src.common import mongo


def get_user_armoury(uid):
    """ Fetch and return all armoury items belonging to the user. """
    return list(mongo.db["userArmouryItems"].find({"userId": uid}))


def update_one_item(uid, iid, inc, *, upsert: bool = True):
    """ Update a single document which shares the same owner and item identifier. """
    mongo.db["userArmouryItems"].update_one(
        {"userId": uid, "itemId": iid},
        {"$inc": inc},
        upsert=upsert
    )
