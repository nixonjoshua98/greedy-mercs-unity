from __future__ import annotations

from typing import Optional

from bson import ObjectId
from pymongo import ReturnDocument

from src.request import ServerRequest
from src.shared_models import BaseDocument, Field
from src.static_models.armoury import ArmouryItemID


def get_armoury_repository(request: ServerRequest) -> ArmouryRepository:
    return ArmouryRepository(request.app.state.mongo)


class ArmouryItemFields:
    user_id = "userId"
    item_id = "itemId"
    level = "level"
    merge_level = "mergeLevel"
    owned = "owned"


class ArmouryItemModel(BaseDocument):
    user_id: ObjectId = Field(..., alias=ArmouryItemFields.user_id)
    item_id: ArmouryItemID = Field(..., alias=ArmouryItemFields.item_id)

    level: int = Field(1, alias=ArmouryItemFields.level)
    owned: int = Field(..., alias=ArmouryItemFields.owned)
    merge_lvl: int = Field(0, alias=ArmouryItemFields.merge_level)


class ArmouryRepository:
    def __init__(self, client):
        self._items = client.database["armouryItems"]

    async def get_user_item(self, uid: ObjectId, iid: ArmouryItemID) -> Optional[ArmouryItemModel]:
        item = await self._items.find_one(self._unique_item(uid, iid))

        return ArmouryItemModel.parse_obj(item) if item is not None else item

    async def get_user_items(self, uid: ObjectId) -> list[ArmouryItemModel]:
        ls = await self._items.find({ArmouryItemFields.user_id: uid}).to_list(length=None)

        return [ArmouryItemModel.parse_obj(ele) for ele in ls]

    async def inc_item_owned(self, uid: ObjectId, iid: ArmouryItemID, val: int) -> Optional[ArmouryItemModel]:
        return await self._update_item(uid, iid, {"$inc": {ArmouryItemFields.owned: val}}, upsert=True)

    async def inc_item_level(self, uid: ObjectId, iid: ArmouryItemID, val: int) -> Optional[ArmouryItemModel]:
        return await self._update_item(uid, iid, {"$inc": {ArmouryItemFields.level: val}}, upsert=False)

    async def inc_merge_item_level(self, uid: ObjectId, iid: ArmouryItemID, val: int) -> Optional[ArmouryItemModel]:
        return await self._update_item(uid, iid, {"$inc": {ArmouryItemFields.merge_level: val}}, upsert=False)

    async def _update_item(self, uid, iid: ArmouryItemID, update: dict, *, upsert: bool) -> Optional[ArmouryItemModel]:
        r = await self._items.find_one_and_update(
            self._unique_item(uid, iid),
            update,
            upsert=upsert,
            return_document=ReturnDocument.AFTER
        )

        return ArmouryItemModel.parse_obj(r) if r is not None else None

    @staticmethod
    def _unique_item(uid: ObjectId, iid: ArmouryItemID):
        return {ArmouryItemFields.user_id: uid, ArmouryItemFields.item_id: iid}
