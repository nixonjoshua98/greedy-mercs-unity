
from src.common import mongo


class CompleteUserData:
    def __init__(self, uid):
        self.uid = uid

        self.armoury = UserArmouryData(uid)


class UserArmouryData:
    def __init__(self, uid):
        self.uid = uid

    def as_list(self):
        return list(mongo.db["userArmouryItems"].find({"userId": self.uid}))

    def update(self, iid: int, inc_: dict, *, upsert: bool = False):
        mongo.db["userArmouryItems"].update_one(
            {"userId": self.uid, "itemId": iid},
            {"$inc": inc_},
            upsert=upsert
        )
