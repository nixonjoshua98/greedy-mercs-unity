from __future__ import annotations

from typing import Optional, Union

from bson import ObjectId
from pymongo import ReturnDocument

from src.pymodels import Field, BaseModel
from src.routing import ServerRequest


def armoury_repository(request: ServerRequest) -> ArmouryRepository:
    return ArmouryRepository(request.app.state.mongo)


class ArmouryItemModel(BaseModel):

    class Aliases:
        USER_ID = "userId"
        ITEM_ID = "itemId"
        LEVEL = "level"
        MERGE_LEVEL = "mergeLevel"
        NUM_OWNED = "owned"

    user_id: ObjectId = Field(..., alias=Aliases.USER_ID)
    item_id: int = Field(..., alias=Aliases.ITEM_ID)

    level: int = Field(1, alias=Aliases.LEVEL)
    owned: int = Field(..., alias=Aliases.NUM_OWNED)
    merge_lvl: int = Field(0, alias=Aliases.MERGE_LEVEL)

    def client_dict(self):
        return self.dict(exclude={"user_id"})


class ArmouryRepository:
    def __init__(self, client):
        self._col = client.database["armouryItems"]

    async def get_user_item(self, uid, iid) -> Union[ArmouryItemModel, None]:
        item = await self._col.find_one({
            ArmouryItemModel.Aliases.USER_ID: uid,
            ArmouryItemModel.Aliases.ITEM_ID: iid
        })

        return ArmouryItemModel.parse_obj(item) if item is not None else item

    async def get_user_items(self, uid) -> list[ArmouryItemModel]:
        ls = await self._col.find({
            ArmouryItemModel.Aliases.USER_ID: uid
        }).to_list(length=None)

        return [ArmouryItemModel.parse_obj(ele) for ele in ls]

    async def update_item(self, uid, iid: int, update: dict, *, upsert: bool) -> Optional[ArmouryItemModel]:
        r = await self._col.find_one_and_update({
            ArmouryItemModel.Aliases.USER_ID: uid,
            ArmouryItemModel.Aliases.ITEM_ID: iid
        },
            update,
            upsert=upsert,
            return_document=ReturnDocument.AFTER,
        )

        return ArmouryItemModel.parse_obj(r) if r is not None else None

    async def inc_item_owned(self, uid: ObjectId, iid: int, val: int) -> Optional[ArmouryItemModel]:
        return await self.update_item(uid, iid, {"$inc": {ArmouryItemModel.Aliases.NUM_OWNED: val}}, upsert=True)

    async def inc_item_level(self, uid: ObjectId, iid: int, val: int) -> Optional[ArmouryItemModel]:
        return await self.update_item(uid, iid, {"$inc": {ArmouryItemModel.Aliases.LEVEL: val}}, upsert=False)

    async def inc_merge_item_level(self, uid: ObjectId, iid: int, val: int) -> Optional[ArmouryItemModel]:
        return await self.update_item(uid, iid, {"$inc": {ArmouryItemModel.Aliases.MERGE_LEVEL: val}}, upsert=False)
