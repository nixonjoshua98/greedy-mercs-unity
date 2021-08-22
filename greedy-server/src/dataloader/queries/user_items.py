from bson import ObjectId
from typing import Union

from pymongo import ReturnDocument

from src.common.enums import ItemKey
from .query_container import DatabaseQueryContainer


class UserItemQueryContainer(DatabaseQueryContainer):
    async def get_items(self, uid: Union[str, ObjectId], *, post_process: bool = True) -> dict:
        result = (await self.client.get_default_database()["userItems"].find_one({"userId": uid})) or dict()

        return await self._after_find(result) if post_process else result

    async def get_item(self, uid: Union[str, ObjectId], key: str, *, post_process=True) -> int:
        return (await self.get_items(uid, post_process=post_process)).get(key, 0)

    async def update_and_get(self, uid: Union[str, ObjectId], update: dict) -> dict:
        return await self._after_find(
            await self.client.get_default_database()["userItems"].find_one_and_update(
                {"userId": uid}, await self._before_update(uid, update),
                upsert=True, return_document=ReturnDocument.AFTER
            )
        )

    async def _before_update(self, uid: Union[str, ObjectId], update: dict):
        update = await self._move_inc_to_set(uid, update, ItemKey.PRESTIGE_POINTS)

        return update

    @staticmethod
    async def _after_find(result: dict):
        """ Perform datatype conversions (ex. string to BigInteger) """

        result[ItemKey.PRESTIGE_POINTS] = int(result.pop(ItemKey.PRESTIGE_POINTS, 0))

        return result

    async def _move_inc_to_set(self, uid: Union[str, ObjectId], update: dict, key: str):
        """
        This will convert a $inc statement to a $set statement. This is used on fields which are stored as
        a string since they exceed Mongo max integer. This conversion requires an addition database query
        and is not an ideal work around. Note: Look into storing a mantissa/exponent
        """
        if inc_amount := update["$inc"].get(key):
            amount = await self.get_item(uid, key)

            update["$set"] = update.get("$set", dict())
            update["$set"][key] = str(amount + inc_amount)

            update["$inc"].pop(key)

        if not update["$inc"]:
            update.pop("$inc")

        return update
