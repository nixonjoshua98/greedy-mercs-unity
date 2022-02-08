from __future__ import annotations

import datetime as dt
from typing import Optional

from bson import ObjectId
from pydantic import Field
from pymongo import ReturnDocument

from src.pymodels import BaseDocument
from src.routing import ServerRequest


def artefacts_repository(request: ServerRequest) -> ArtefactsRepository:
    return ArtefactsRepository(request.app.state.mongo)


class ArtefactModel(BaseDocument):

    class Aliases:
        ARTEFACT_ID = "artefactId"
        USER_ID = "userId"
        LEVEL = "level"
        UNLOCK_TIME = "unlockTime"

    artefact_id: int = Field(..., alias=Aliases.ARTEFACT_ID)
    user_id: ObjectId = Field(..., alias=Aliases.USER_ID)

    level: int = Field(..., alias=Aliases.LEVEL)

    def client_dict(self):
        return self.dict(exclude={"id", "user_id"})


class ArtefactsRepository:
    def __init__(self, client):
        self._col = client.database["userArtefacts"]

    async def get_all_artefacts(self, uid) -> list[ArtefactModel]:
        ls = await self._col.find({ArtefactModel.Aliases.USER_ID: uid}).to_list(length=None)

        return [ArtefactModel.parse_obj(ele) for ele in ls]

    async def inc_level(self, uid: ObjectId, art_id: int, levels: int) -> Optional[ArtefactModel]:
        return await self.update_artefact(uid, art_id, {"$inc": {ArtefactModel.Aliases.LEVEL: levels}})

    async def get_artefact(self, uid, artid) -> Optional[ArtefactModel]:
        r = await self._col.find_one({
            ArtefactModel.Aliases.USER_ID: uid,
            ArtefactModel.Aliases.ARTEFACT_ID: artid
        })

        return ArtefactModel.parse_obj(r) if r else None

    async def add_new_artefact(self, uid, artid) -> Optional[ArtefactModel]:
        await self._col.insert_one({
            ArtefactModel.Aliases.USER_ID: uid,
            ArtefactModel.Aliases.ARTEFACT_ID: artid,
            ArtefactModel.Aliases.LEVEL: 1,
            ArtefactModel.Aliases.UNLOCK_TIME: dt.datetime.utcnow(),
            })

        return await self.get_artefact(uid, artid)

    async def update_artefact(self, uid, artid, update: dict, *, upsert: bool = False) -> Optional[ArtefactModel]:
        r = await self._col.find_one_and_update({
            ArtefactModel.Aliases.USER_ID: uid,
            ArtefactModel.Aliases.ARTEFACT_ID: artid
        },
            update,
            upsert=upsert,
            return_document=ReturnDocument.AFTER,
        )

        return ArtefactModel.parse_obj(r) if r else None
