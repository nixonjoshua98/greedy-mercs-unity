from __future__ import annotations

import datetime as dt
from typing import Optional

from bson import ObjectId
from pydantic import Field
from pymongo import ReturnDocument, UpdateOne

from src.pymodels import BaseModel
from src.request import ServerRequest
from src.static_models.artefacts import ArtefactID


def artefacts_repository(request: ServerRequest) -> ArtefactsRepository:
    return ArtefactsRepository(request.app.state.mongo)


class Fields:
    artefact_id = "artefactId"
    user_id = "userId"
    level = "level"
    unlock_time = "unlockTime"


class ArtefactModel(BaseModel):
    artefact_id: ArtefactID = Field(..., alias=Fields.artefact_id)
    user_id: ObjectId = Field(..., alias=Fields.user_id)

    level: int = Field(..., alias=Fields.level)


class ArtefactsRepository:
    def __init__(self, client):
        self._artefacts = client.database["userArtefacts"]

    async def bulk_upgrade(self, uid: ObjectId, upgrades: dict[ArtefactID, int]):
        requests = [UpdateOne({Fields.user_id: uid, Fields.artefact_id: aid},
                              {"$inc": {Fields.level: level}})
                    for aid, level in upgrades.items()]

        await self._artefacts.bulk_write(requests)

    async def get_user_artefacts(self, uid) -> list[ArtefactModel]:
        ls = await self._artefacts.find({Fields.user_id: uid}).to_list(length=None)

        return [ArtefactModel.parse_obj(ele) for ele in ls]

    async def inc_level(self, uid: ObjectId, art_id: int, levels: int) -> Optional[ArtefactModel]:
        return await self.update_artefact(uid, art_id, {"$inc": {Fields.level: levels}})

    async def get_artefact(self, uid, artid) -> Optional[ArtefactModel]:
        r = await self._artefacts.find_one({Fields.user_id: uid, Fields.artefact_id: artid})

        return ArtefactModel.parse_obj(r) if r else None

    async def add_new_artefact(self, uid, artid) -> Optional[ArtefactModel]:
        await self._artefacts.insert_one(
            {
                Fields.user_id: uid,
                Fields.artefact_id: artid,
                Fields.level: 1,
                Fields.unlock_time: dt.datetime.utcnow(),
            }
        )

        return await self.get_artefact(uid, artid)

    async def update_artefact(self, uid, artid, update: dict, *, upsert: bool = False) -> Optional[ArtefactModel]:
        r = await self._artefacts.find_one_and_update(
            {Fields.user_id: uid, Fields.artefact_id: artid},
            update,
            upsert=upsert,
            return_document=ReturnDocument.AFTER,
        )

        return ArtefactModel.parse_obj(r) if r else None
