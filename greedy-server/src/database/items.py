from pymongo import ReturnDocument

from src.common.enums import ItemKeys


class Items:
    __collection__ = "userItems"

    def __init__(self, mongoc):
        self.collection = mongoc.db[self.__collection__]

    def get_items(self, uid, *, post_process: bool = True) -> dict:
        """ Retrieve all user items

        Args:
            uid: User
            post_process (bool): Choose to process the result (ex. convert string number to integer)

        Returns:
            dict: User items
        """
        result = self.collection.find_one({"userId": uid}) or dict()

        return self._after_find(result) if post_process else result

    def get_item(self, uid, key: str) -> int:
        """ Retrieve a single item

        Args:
            uid: User
            key (str): Key which we want to get

        Returns:
            dict: Single user item
        """
        return self.get_items(uid).get(key, 0)

    def update_one(self, uid, update: dict, *, upsert: bool = True) -> bool:
        """ Update a single document, with the uid

        Args:
            uid: User
            update (dict): The update part of the Mongo query
            upsert (bool): Choose to insert a new row, if an existing one does not exist

        Returns:
            bool: Whether a document was modified
        """
        result = self.collection.update_one({"userId": uid}, self._before_update(uid, update), upsert=upsert)

        return result.modified_count > 0

    def update_and_find(self, uid, update: dict) -> dict:
        """ Update a single document, with the uid, and return the document

        Args:
            uid: User
            update (dict): The update part of the Mongo query

        Returns:
            dict: User's items AFTER the query was executed
        """
        return self._after_find(
            self.collection.find_one_and_update(
                {"userId": uid}, self._before_update(uid, update), upsert=True, return_document=ReturnDocument.AFTER
            )
        )

    def _before_update(self, uid, update: dict):

        update = self._move_inc_to_set(uid, update, ItemKeys.PRESTIGE_POINTS)

        return update

    @staticmethod
    def _after_find(result: dict):
        """ Perform datatype conversions (ex. string to BigInteger) """

        result[ItemKeys.PRESTIGE_POINTS] = int(result.pop(ItemKeys.PRESTIGE_POINTS, 0))

        return result

    def _move_inc_to_set(self, uid, update: dict, key: str):
        """
        This will convert a $inc statement to a $set statement. This is used on fields which are stored as
        a string since they exceed Mongo max integer. This conversion requires an addition database query
        and is not an ideal work around. Note: Look into storing a mantissa/exponent
        """
        if inc_amount := update["$inc"].get(key):
            has_amount = self.get_item(uid, key)

            update["$set"] = update.get("$set", dict())
            update["$set"][key] = str(has_amount + inc_amount)

            update["$inc"].pop(key)

        if not update["$inc"]:
            update.pop("$inc")

        return update
