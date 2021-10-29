from __future__ import annotations

from typing import Union

from bson import ObjectId
from pydantic import Field
from pymongo import ReturnDocument

from src.pymodels import BaseDocument
from src.routing import ServerRequest


def inject_armoury_repository(request: ServerRequest) -> ArmouryRepository:
    return ArmouryRepository(request.app.state.mongo)


# === Fields === #


class Fields:
    USER_ID = "userId"
    ITEM_ID = "itemId"
    LEVEL = "level"
    MERGE_LEVEL = "mergeLevel"
    NUM_OWNED = "owned"


# == Models == #


class ArmouryItemModel(BaseDocument):
    user_id: ObjectId = Field(..., alias=Fields.USER_ID)
    item_id: int = Field(..., alias=Fields.ITEM_ID)

    level: int = Field(1, alias=Fields.LEVEL)
    owned: int = Field(..., alias=Fields.NUM_OWNED)
    merge_lvl: int = Field(0, alias=Fields.MERGE_LEVEL)

    def response_dict(self):
        return self.dict(exclude={"id", "user_id"})


# == Repository == #


class ArmouryRepository:
    def __init__(self, client):
        db = client.get_default_database()

        self._col = db["armouryItems"]

    async def get_one_user_item(self, uid, iid) -> Union[ArmouryItemModel, None]:
        item = await self._col.find_one({Fields.USER_ID: uid, Fields.ITEM_ID: iid})

        return ArmouryItemModel.parse_obj(item) if item is not None else item

    async def get_all_user_items(self, uid) -> list[ArmouryItemModel]:
        ls = await self._col.find({Fields.USER_ID: uid}).to_list(length=None)

        return [ArmouryItemModel.parse_obj(ele) for ele in ls]

    async def update_item(
        self, uid, iid: int, update: dict, *, upsert: bool
    ) -> Union[ArmouryItemModel, None]:
        r = await self._col.find_one_and_update(
            {Fields.USER_ID: uid, Fields.ITEM_ID: iid},
            update,
            upsert=upsert,
            return_document=ReturnDocument.AFTER,
        )

        return ArmouryItemModel.parse_obj(r) if r is not None else None
