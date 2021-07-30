from pymongo import ReturnDocument


class ItemKeys:
    BOUNTY_POINTS = "bountyPoints"
    ARMOURY_POINTS = "ironIngots"
    PRESTIGE_POINTS = "prestigePoints"


class Items:
    __collection__ = "userItems"

    def __init__(self, mongoc):
        self.collection = mongoc.db[self.__collection__]

    def get_items(self, uid, *, post_process: bool = True) -> dict:
        result = self.collection.find_one({"userId": uid}) or dict()

        return self._after_find(result) if post_process else result

    def get_item(self, uid, key) -> int:
        return (self.collection.find_one({"userId": uid}) or dict()).get(key, 0)

    def update_one(self, uid, update: dict, *, upsert: bool = True) -> bool:
        result = self.collection.update_one({"userId": uid}, self._before_update(uid, update), upsert=upsert)

        return result.modified_count > 0

    def update_and_find(self, uid, update: dict) -> dict:
        return self._after_find(
            self.collection.find_one_and_update(
                {"userId": uid}, self._before_update(uid, update), upsert=True, return_document=ReturnDocument.AFTER
            )
        )

    def _before_update(self, uid, update: dict):

        update = self._move_inc_to_set(uid, update, ItemKeys.PRESTIGE_POINTS)

        return update

    def _move_inc_to_set(self, uid, update: dict, key: str):

        if inc_amount := update["$inc"].get(key):
            has_amount = self.get_item(uid, key)

            update["$set"] = update.get("$set", dict())
            update["$set"] = {key: str(int(has_amount) + inc_amount)}

            update["$inc"].pop(key)

        if not update["$inc"]:
            update.pop("$inc")

        return update

    @staticmethod
    def _after_find(result: dict):
        """ Perform datatype conversions (ex. string to BigInteger) """

        result[ItemKeys.PRESTIGE_POINTS] = int(result.pop(ItemKeys.PRESTIGE_POINTS, 0))

        return result


