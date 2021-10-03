from typing import Union
from bson import ObjectId
from pymongo import ReturnDocument
from motor.motor_asyncio import AsyncIOMotorClient

import datetime as dt

from src import resources
from src.common.enums import ItemKey

from src.classes.serverstate import ServerState
from src.classes.bountyshop import AbstractBountyShopItem

from src.mongo.repositories import BountiesRepository


class DataLoader:
    def __init__(self):
        state = ServerState()

        db = self._mongo.get_default_database()

        self.items = _Items(db)
        self.users = _Users(db)
        self.armoury = _Armoury(db)
        self.bounties = _Bounties(db)
        self.artefacts = _Artefacts(db)
        self.bounty_shop = _BountyShop(db, prev_daily_reset=state.prev_daily_reset)

    @classmethod
    def create_client(cls, con_str):
        cls._mongo = AsyncIOMotorClient(con_str)

    @classmethod
    def get_client(cls):
        return getattr(cls, "_mongo", None)

    def __enter__(self):
        return self

    def __exit__(self, exc_type, exc_val, exc_tb): ...

    async def get_user_data(self, uid):
        return {
            "inventory": {
                "items": await self.items.get_items(uid, post_process=False)
            },

            "bountyShop": {
                "dailyPurchases": await self.bounty_shop.get_daily_purchases(uid),
                "availableItems": resources.get_bounty_shop(uid, as_list=True),
            },

            "bounties_userData": await self.bounties.find_one(uid),

            "armoury_userData": await self.armoury.get_all_items(uid),
            "artefacts_userData": await self.artefacts.get_all_artefacts(uid),
        }


class _Users:
    def __init__(self, default_database):
        self.collection = default_database["userLogins"]

    async def get_user(self, device_id: str):
        return await self.collection.find_one({"deviceId": device_id})

    async def insert_new_user(self, data: dict):
        r = await self.collection.insert_one(data)

        return r.inserted_id


class _Bounties:
    """
    _id
    userId
    lastClaimTime
    bounties: [
        {"bountyId": 0, "isActive": False},
        ]
    """

    def __init__(self, default_database):
        self._data = default_database["userBountiesData"]

    async def find_one(self, uid):
        return await self._data.find_one_and_update(
            {"_id": uid}, {
                "$setOnInsert": {
                    "lastClaimTime": dt.datetime.utcnow()
                }
            },
            return_document=ReturnDocument.AFTER,
            upsert=True
        )

    async def add_new_bounty(self, uid, bid):
        await self._data.update_one(
            {"_id": uid, "bounties": {"$not": {"$elemMatch": {"bountyId": bid}}}},
            {
                "$addToSet": {
                    "bounties": {
                        "bountyId": bid,
                        "isActive": False
                    }
                }},
            upsert=True)

    async def update_active_bounties(self, uid, ids: list):
        result = await self.find_one(uid)

        bounties = result.get("bounties", [])

        for b in bounties:
            is_active = b["bountyId"] in ids

            await self._data.update_one(
                {"_id": result["_id"], "bounties.bountyId": b["bountyId"]},
                {"$set": {"bounties.$.isActive": is_active}}
            )

    async def get_user_bounties(self, uid: Union[str, ObjectId]) -> dict:
        return (await self.find_one(uid)).get("bounties", [])

    async def set_claim_time(self, uid: Union[str, ObjectId], claim_time: dt.datetime) -> None:
        await self._data.update_one({"_id": uid}, {"$set": {"lastClaimTime": claim_time}}, upsert=True)


class _Items:
    def __init__(self, default_database):
        self.collection = default_database["userItems"]

    async def get_items(self, uid: Union[str, ObjectId], *, post_process: bool = True) -> dict:
        result = (await self.collection.find_one({"userId": uid})) or dict()

        return await self._after_find(result) if post_process else result

    async def get_item(self, uid: Union[str, ObjectId], key: str, *, default=0, post_process=True) -> int:
        return (await self.get_items(uid, post_process=post_process)).get(key, default)

    async def update_items(self, uid: Union[str, ObjectId], update: dict, *, upsert: bool = True) -> bool:
        return (await self.collection.update_one(
            {"userId": uid}, await self._before_update(uid, update), upsert=upsert
        )).modified_count > 0

    async def update_and_get(self, uid: Union[str, ObjectId], update: dict) -> dict:
        return await self._after_find(
            await self.collection.find_one_and_update(
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


class _BountyShop:
    def __init__(self, default_database, prev_daily_reset):
        self.prev_daily_reset = prev_daily_reset

        self.collection = default_database["bountyShopPurchases"]

    async def log_purchase(self, uid, item: AbstractBountyShopItem):
        await self.collection.insert_one(
            {"userId": uid, "itemId": item.id, "purchaseTime": dt.datetime.utcnow(), "itemData": item.to_dict()}
        )

    async def get_daily_purchases(self, uid, iid: int = None) -> Union[dict, int]:
        """ Count the number of purchase made for an item (if provided) by a user since the previous reset. """

        filter_ = {"userId": uid, "purchaseTime": {"$gte": self.prev_daily_reset}}

        if iid is not None:
            filter_["itemId"] = iid

        results = await self.collection.find(filter_).to_list(length=None)

        def count(item_id: int):
            return len([r for r in results if r["itemId"] == item_id])

        data = {iid_: count(iid_) for iid_ in set(r["itemId"] for r in results)}

        return data.get(iid, 0) if iid is not None else data
