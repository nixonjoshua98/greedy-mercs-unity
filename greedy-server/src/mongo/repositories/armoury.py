from __future__ import annotations

from typing import Optional, Union

from bson import ObjectId
from pymongo import ReturnDocument

from src.pymodels import BaseDocument, Field
from src.routing import ServerRequest


def armoury_repository(request: ServerRequest) -> ArmouryRepository:
    return ArmouryRepository(request.app.state.mongo)


class Fields:
    USER_ID = "userId"
    ITEM_ID = "itemId"
    LEVEL = "level"
    MERGE_LEVEL = "mergeLevel"
    NUM_OWNED = "owned"


class ArmouryItemModel(BaseDocument):
    user_id: ObjectId = Field(..., alias=Fields.USER_ID)
    item_id: int = Field(..., alias=Fields.ITEM_ID)

    level: int = Field(1, alias=Fields.LEVEL)
    owned: int = Field(..., alias=Fields.NUM_OWNED)
    merge_lvl: int = Field(0, alias=Fields.MERGE_LEVEL)

    def client_dict(self):
        return self.dict(exclude={"id", "user_id"})


class ArmouryRepository:
    def __init__(self, client):
        self._col = client.database["armouryItems"]

    async def get_user_item(self, uid, iid) -> Union[ArmouryItemModel, None]:
        item = await self._col.find_one({Fields.USER_ID: uid, Fields.ITEM_ID: iid})

        return ArmouryItemModel.parse_obj(item) if item is not None else item

    async def get_user_items(self, uid) -> list[ArmouryItemModel]:
        ls = await self._col.find({Fields.USER_ID: uid}).to_list(length=None)

        return [ArmouryItemModel.parse_obj(ele) for ele in ls]

    async def update_item(
        self, uid, iid: int, update: dict, *, upsert: bool
    ) -> Optional[ArmouryItemModel]:
        r = await self._col.find_one_and_update(
            {Fields.USER_ID: uid, Fields.ITEM_ID: iid},
            update,
            upsert=upsert,
            return_document=ReturnDocument.AFTER,
        )

        return ArmouryItemModel.parse_obj(r) if r is not None else None

    async def inc_item_owned(
        self, uid: ObjectId, iid: int, val: int
    ) -> Optional[ArmouryItemModel]:
        return await self.update_item(
            uid, iid, {"$inc": {Fields.NUM_OWNED: val}}, upsert=True
        )

    async def inc_item_level(
        self, uid: ObjectId, iid: int, val: int
    ) -> Optional[ArmouryItemModel]:
        return await self.update_item(
            uid, iid, {"$inc": {Fields.LEVEL: val}}, upsert=False
        )

    async def inc_merge_item_level(
        self, uid: ObjectId, iid: int, val: int
    ) -> Optional[ArmouryItemModel]:
        return await self.update_item(
            uid, iid, {"$inc": {Fields.MERGE_LEVEL: val}}, upsert=False
        )
