from __future__ import annotations

import datetime as dt

from bson import ObjectId

from src.pymodels import BaseDocument, Field
from src.routing import ServerRequest


def units_repository(request: ServerRequest) -> CharacterUnitsRepository:
    return CharacterUnitsRepository(request.app.state.mongo)


class Fields:
    USER_ID = "userId"
    UNIT_ID = "unitId"
    UNLOCK_TIME = "unlockTime"


class CharacterUnitModel(BaseDocument):
    user_id: ObjectId = Field(..., alias=Fields.USER_ID)
    unit_id: int = Field(..., alias=Fields.UNIT_ID)
    unlock_time: dt.datetime = Field(..., alias=Fields.UNLOCK_TIME)

    def client_dict(self):
        return self.dict(exclude={"id", "user_id", "unlock_time"})


class CharacterUnitsRepository:
    def __init__(self, client):
        self._col = client.database["userCharacterUnits"]

    async def insert_units(self, uid: ObjectId, unit_ids: list[int]):
        unlocked_ids: list[int] = [u.unit_id for u in await self.get_units(uid)]

        for unit_id in (id_ for id_ in unit_ids if id_ not in unlocked_ids):
            await self._col.update_one(
                {Fields.USER_ID: uid, Fields.UNIT_ID: unit_id},
                {"$setOnInsert": {Fields.UNLOCK_TIME: dt.datetime.utcnow()}},
                upsert=True
            )

    async def get_units(self, uid: ObjectId) -> list[CharacterUnitModel]:
        ls: list[dict] = await self._col.find({Fields.USER_ID: uid}).to_list(length=None)

        return [CharacterUnitModel.parse_obj(ele) for ele in ls]
