from __future__ import annotations

from typing import Union
from pymongo import ReturnDocument
from pydantic import Field
from bson import ObjectId

from src.routing import ServerRequest

from src.common.basemodels import BaseDocument


def armoury_repository(request: ServerRequest) -> ArmouryRepository:
    """ Used to inject a repository instance. """
    return ArmouryRepository(request.app.state.mongo)


# === Fields === #

class Fields:
    USER_ID = "userId"
    ITEM_ID = "itemId"
    LEVEL = "level"
    EVO_LEVEL = "evoLevel"
    NUM_OWNED = "owned"


# == Models == #

class ArmouryItemModel(BaseDocument):
    item_id: int = Field(..., alias=Fields.ITEM_ID)
    user_id: ObjectId = Field(..., alias=Fields.USER_ID)

    level: int = Field(1, alias=Fields.LEVEL)
    owned: int = Field(..., alias=Fields.NUM_OWNED)
    evo_level: int = Field(0, alias=Fields.EVO_LEVEL)

    def response_dict(self):
        return self.dict(exclude={"id", "user_id"})


# == Repository == #

class ArmouryRepository:
    def __init__(self, client):
        db = client.get_default_database()

        self._col = db["armouryItems"]

    async def get_item(self, uid, iid) -> Union[ArmouryItemModel, None]:
        item = await self._col.find_one({Fields.USER_ID: uid, Fields.ITEM_ID: iid})

        return ArmouryItemModel(**item) if item is not None else item

    async def get_all_items(self, uid) -> list[ArmouryItemModel]:
        ls = await self._col.find({Fields.USER_ID: uid}).to_list(length=None)

        return [ArmouryItemModel(**ele) for ele in ls]

    async def update_item(self, uid, iid: int, update: dict) -> ArmouryItemModel:
        r = await self._col.find_one_and_update(
            {
                Fields.USER_ID: uid,
                Fields.ITEM_ID: iid
            }, update, upsert=False, return_document=ReturnDocument.AFTER)

        return ArmouryItemModel(**r)
