from __future__ import annotations

import datetime as dt
from typing import Optional, Union

from bson import ObjectId
from pydantic import Field
from pymongo import ReturnDocument

from src.pymodels import BaseDocument
from src.routing import ServerRequest


def artefacts_repository(request: ServerRequest) -> ArtefactsRepository:
    return ArtefactsRepository(request.app.state.mongo)


class Fields:
    ARTEFACT_ID = "artefactId"
    USER_ID = "userId"
    LEVEL = "level"
    UNLOCK_TIME = "unlockTime"


class ArtefactModel(BaseDocument):
    artefact_id: int = Field(..., alias=Fields.ARTEFACT_ID)
    user_id: ObjectId = Field(..., alias=Fields.USER_ID)

    level: int = Field(..., alias=Fields.LEVEL)

    def client_dict(self):
        return self.dict(exclude={"id", "user_id"})


class ArtefactsRepository:
    def __init__(self, client):
        self._col = client.database["userArtefacts"]

    async def get_all_artefacts(self, uid) -> list[ArtefactModel]:
        ls = await self._col.find({Fields.USER_ID: uid}).to_list(length=None)

        return [ArtefactModel.parse_obj(ele) for ele in ls]

    async def inc_level(self, uid: ObjectId, art_id: int, levels: int) -> Optional[ArtefactModel]:
        return await self.update_artefact(uid, art_id, {"$inc": {Fields.LEVEL: levels}})

    async def get_artefact(self, uid, artid) -> Optional[ArtefactModel]:
        r = await self._col.find_one({Fields.USER_ID: uid, Fields.ARTEFACT_ID: artid})

        return ArtefactModel.parse_obj(r) if r else None

    async def add_new_artefact(self, uid, artid) -> Optional[ArtefactModel]:
        await self._col.insert_one(
            {
                Fields.USER_ID: uid,
                Fields.ARTEFACT_ID: artid,
                Fields.LEVEL: 1,
                Fields.UNLOCK_TIME: dt.datetime.utcnow(),
            }
        )

        return await self.get_artefact(uid, artid)

    async def update_artefact(self, uid, artid, update: dict, *, upsert: bool = False) -> Optional[ArtefactModel]:
        r = await self._col.find_one_and_update(
            {Fields.USER_ID: uid, Fields.ARTEFACT_ID: artid},
            update,
            upsert=upsert,
            return_document=ReturnDocument.AFTER,
        )

        return ArtefactModel.parse_obj(r) if r else None
