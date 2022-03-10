from __future__ import annotations

from typing import Optional

from bson import ObjectId
from pymongo import ReturnDocument

from src.pymodels import BaseDocument, Field
from src.request import ServerRequest
from src.static_models.armoury import ArmouryItemID


def armoury_repository(request: ServerRequest) -> ArmouryRepository:
    return ArmouryRepository(request.app.state.mongo)


class ArmouryItemModel(BaseDocument):
    class Aliases:
        USER_ID = "userId"
        ITEM_ID = "itemId"
        LEVEL = "level"
        MERGE_LEVEL = "mergeLevel"
        NUM_OWNED = "owned"

    user_id: ObjectId = Field(..., alias=Aliases.USER_ID)
    item_id: ArmouryItemID = Field(..., alias=Aliases.ITEM_ID)

    level: int = Field(1, alias=Aliases.LEVEL)
    owned: int = Field(..., alias=Aliases.NUM_OWNED)
    merge_lvl: int = Field(0, alias=Aliases.MERGE_LEVEL)

    def client_dict(self):
        return self.dict(exclude={"id", "user_id"})


class ArmouryRepository:
    def __init__(self, client):
        self._col = client.database["armouryItems"]

    async def get_user_item(self, uid: ObjectId, iid: ArmouryItemID) -> Optional[ArmouryItemModel]:
        item = await self._col.find_one(self._unique_item(uid, iid))

        return ArmouryItemModel.parse_obj(item) if item is not None else item

    async def get_user_items(self, uid) -> list[ArmouryItemModel]:
        ls = await self._col.find({ArmouryItemModel.Aliases.USER_ID: uid}).to_list(length=None)

        return [ArmouryItemModel.parse_obj(ele) for ele in ls]

    async def inc_item_owned(self, uid: ObjectId, iid: ArmouryItemID, val: int) -> Optional[ArmouryItemModel]:
        return await self._update_item(uid, iid, {"$inc": {ArmouryItemModel.Aliases.NUM_OWNED: val}}, upsert=True)

    async def inc_item_level(self, uid: ObjectId, iid: ArmouryItemID, val: int) -> Optional[ArmouryItemModel]:
        return await self._update_item(uid, iid, {"$inc": {ArmouryItemModel.Aliases.LEVEL: val}}, upsert=False)

    async def inc_merge_item_level(self, uid: ObjectId, iid: ArmouryItemID, val: int) -> Optional[ArmouryItemModel]:
        return await self._update_item(uid, iid, {"$inc": {ArmouryItemModel.Aliases.MERGE_LEVEL: val}}, upsert=False)

    async def _update_item(self, uid, iid: ArmouryItemID, update: dict, *, upsert: bool) -> Optional[ArmouryItemModel]:
        r = await self._col.find_one_and_update(
            self._unique_item(uid, iid),
            update,
            upsert=upsert,
            return_document=ReturnDocument.AFTER
        )

        return ArmouryItemModel.parse_obj(r) if r is not None else None

    @staticmethod
    def _unique_item(uid: ObjectId, iid: ArmouryItemID):
        return {ArmouryItemModel.Aliases.USER_ID: uid, ArmouryItemModel.Aliases.ITEM_ID: iid}
