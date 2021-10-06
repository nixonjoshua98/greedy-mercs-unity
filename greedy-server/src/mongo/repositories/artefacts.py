from __future__ import annotations

from typing import Union
from bson import ObjectId
from pydantic import Field
from pymongo import ReturnDocument

import datetime as dt

from src.routing import ServerRequest

from src.common.basemodels import BaseDocument


def artefacts_repository(request: ServerRequest) -> ArtefactsRepository:
    """ Used to inject a repository instance. """
    return ArtefactsRepository(request.app.state.mongo)

# == Fields == #


class Fields:
    ARTEFACT_ID = "artefactId"
    USER_ID = "userId"
    LEVEL = "level"
    UNLOCK_TIME = "unlockTime"


# == Models == #

class ArtefactModel(BaseDocument):
    artefact_id: int = Field(..., alias=Fields.ARTEFACT_ID)
    user_id: ObjectId = Field(..., alias=Fields.USER_ID)

    level: int = Field(..., alias=Fields.LEVEL)

    def response_dict(self):
        return self.dict(exclude={"id", "user_id"})


# == Repository == #

class ArtefactsRepository:
    def __init__(self, client):
        db = client.get_default_database()

        self._col = db["userArtefacts"]

    async def get_all_artefacts(self, uid) -> list[ArtefactModel]:
        ls = await self._col.find({Fields.USER_ID: uid}).to_list(length=None)

        return [ArtefactModel(**ele) for ele in ls]

    async def get_artefact(self, uid, artid) -> Union[ArtefactModel, None]:
        r = await self._col.find_one({Fields.USER_ID: uid, Fields.ARTEFACT_ID: artid})

        return ArtefactModel(**r) if r is not None else None

    async def add_new_artefact(self, uid, artid) -> ArtefactModel:
        r = await self._col.insert_one(doc := {
            Fields.LEVEL: 1,
            Fields.USER_ID: uid,
            Fields.ARTEFACT_ID: artid,
            Fields.UNLOCK_TIME: dt.datetime.utcnow()
        })

        return ArtefactModel(**{"_id": r.inserted_id, **doc})

    async def update_artefact(self, uid, artid, update: dict) -> ArtefactModel:
        r = await self._col.find_one_and_update({
                Fields.USER_ID: uid,
                Fields.ARTEFACT_ID: artid
            }, update, upsert=False, return_document=ReturnDocument.AFTER)

        return ArtefactModel(**r)
