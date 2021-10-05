from typing import Union
from bson import ObjectId
from pymongo import ReturnDocument
from motor.motor_asyncio import AsyncIOMotorClient

import datetime as dt

from src import resources
from src.common.enums import ItemKey

from src.classes.serverstate import ServerState
from src.classes.bountyshop import AbstractBountyShopItem


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

    def __enter__(self):
        return self

    def __exit__(self, exc_type, exc_val, exc_tb): ...

    async def get_user_data(self, uid):
        return {
            "inventory": {
                "items": await self.items.get_items(uid)
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
    def __init__(self, default_database):
        self._data = default_database["userBounties"]

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


class _Items:
    def __init__(self, default_database):
        self.collection = default_database["userItems"]

    async def get_items(self, uid: Union[str, ObjectId], *, post_process: bool = True) -> dict:
        return (await self.collection.find_one({"userId": uid})) or dict()

    async def get_item(self, uid: Union[str, ObjectId], key: str, *, default=0, post_process=True) -> int:
        return (await self.get_items(uid, post_process=post_process)).get(key, default)

    async def update_items(self, uid: Union[str, ObjectId], update: dict, *, upsert: bool = True):
        await self.collection.update_one({"userId": uid}, update, upsert=upsert)

    async def update_and_get(self, uid: Union[str, ObjectId], update: dict) -> dict:
        return await self.collection.find_one_and_update(
            {"userId": uid}, update,
            upsert=True, return_document=ReturnDocument.AFTER
        )


class _Armoury:
    def __init__(self, default_database):
        self.collection = default_database["armouryItems"]

    async def update_one_item(self, uid: Union[str, ObjectId], iid: int, update: dict, *, upsert: bool) -> bool:
        result = await self.collection.update_one({"userId": uid, "itemId": iid}, update, upsert=upsert)

        return result.modified_count > 0

    async def get_all_items(self, uid) -> dict:
        ls = await self.collection.find({"userId": uid}).to_list(length=None)

        return {ele["itemId"]: ele for ele in ls}


class _Artefacts:
    def __init__(self, default_database):
        self.collection = default_database["userArtefacts"]

    async def get_all_artefacts(self, uid: Union[ObjectId, str]) -> dict:
        result = await self.collection.find({"userId": uid}).to_list(length=None)

        return {ele["artefactId"]: ele for ele in result}


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
