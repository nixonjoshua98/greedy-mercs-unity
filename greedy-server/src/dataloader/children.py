from typing import Union
from bson import ObjectId
from pymongo import ReturnDocument

import datetime as dt

from src.common.enums import ItemKey


class _Users:
    def __init__(self, default_database):
        self.default_database = default_database

    async def get_user(self, device_id: str):
        return await self.default_database["userLogins"].find_one({"deviceId": device_id})

    async def insert_new_user(self, row):
        return (await self.default_database["userLogins"].isnert_one(row)).insert_id


class _Bounties:
    def __init__(self, default_database):
        self.default_database = default_database

    async def add_new_bounty(self, uid, bid, *, claim_time=None):
        r = await self.default_database["userBounties"].insert_one(
            {"userId": uid, "bountyId": bid, "lastClaimTime": claim_time or dt.datetime.utcnow()}
        )
        return r.inserted_id  # Return the _id for the new document

    async def get_user_bounties(self, uid: Union[str, ObjectId]) -> dict:
        results = await self.default_database["userBounties"].find({"userId": uid}).to_list(length=None)

        return {x["bountyId"]: x for x in results}

    async def set_all_claim_time(self, uid: Union[str, ObjectId], claim_time: dt.datetime) -> None:
        await self.default_database["userBounties"].update_many(
            {"userId": uid}, {"$set": {"lastClaimTime": claim_time}}
        )


class _Items:
    def __init__(self, default_database):
        self.default_database = default_database

    async def get_items(self, uid: Union[str, ObjectId], *, post_process: bool = True) -> dict:
        result = (await self.default_database["userItems"].find_one({"userId": uid})) or dict()

        return await self._after_find(result) if post_process else result

    async def get_item(self, uid: Union[str, ObjectId], key: str, *, default=0, post_process=True) -> int:
        return (await self.get_items(uid, post_process=post_process)).get(key, default)

    async def update_items(self, uid: Union[str, ObjectId], update: dict, *, upsert: bool = True) -> bool:
        return (await self.default_database["userItems"].update_one(
            {"userId": uid}, await self._before_update(uid, update), upsert=upsert
        )).modified_count > 0

    async def update_and_get(self, uid: Union[str, ObjectId], update: dict) -> dict:
        return await self._after_find(
            await self.default_database["userItems"].find_one_and_update(
                {"userId": uid}, await self._before_update(uid, update),
                upsert=True, return_document=ReturnDocument.AFTER
            )
        )

    @staticmethod
    async def _after_find(result: dict):
        """ Perform datatype conversions (ex. string to integer) """

        result[ItemKey.PRESTIGE_POINTS] = int(result.pop(ItemKey.PRESTIGE_POINTS, 0))

        return result

    async def _before_update(self, uid: Union[str, ObjectId], update: dict):
        update = await self._move_inc_to_set(uid, update, ItemKey.PRESTIGE_POINTS)

        return update

    async def _move_inc_to_set(self, uid: Union[str, ObjectId], update: dict, key: str):
        """
        Replaces a $set update on a string number to a $set for the number so we can
        use $inc for large numbers stored as strings.
        """
        if inc_amount := update["$inc"].pop(key, None):
            amount = await self.get_item(uid, key)

            update["$set"] = update.get("$set", dict())
            update["$set"][key] = str(amount + inc_amount)

        if not update["$inc"]:
            update.pop("$inc")

        return update


class _Armoury:
    def __init__(self, default_database):
        self.collection = default_database["userArmouryItems"]

    async def update_one_item(self, uid: Union[str, ObjectId], iid: int, update: dict, *, upsert: bool) -> bool:
        result = await self.collection.update_one({"userId": uid, "itemId": iid}, update, upsert=upsert)

        return result.modified_count > 0

    async def get_all_items(self, uid) -> dict:
        ls = await self.collection.find({"userId": uid}).to_list(length=None)

        return {ele["itemId"]: ele for ele in ls}

    async def get_one_item(self, uid, iid) -> Union[dict, None]:
        return await self.collection.find_one({"userId": uid, "itemId": iid})


class _Artefacts:
    def __init__(self, default_database):
        self.collection = default_database["userArtefacts"]

    async def get_all_artefacts(self, uid: Union[ObjectId, str]) -> dict:
        result = await self.collection.find({"userId": uid}).to_list(length=None)

        return {ele["artefactId"]: ele for ele in result}

    async def get_one_artefact(self, uid: Union[ObjectId, str], artid) -> dict:
        return (await self.collection.find_one({"userId": uid, "artefactId": artid})) or dict()

    async def add_new_artefact(self, document: dict):
        await self.collection.insert_one(document)

    async def update_one_artefact(self, uid, artid, update: dict, *, upsert: bool = True) -> bool:
        r = await self.collection.update_one({"userId": uid, "artefactId": artid}, update, upsert=upsert)

        return r.modified_count == 1
