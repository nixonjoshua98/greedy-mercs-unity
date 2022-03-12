from __future__ import annotations

from bson import ObjectId
from pydantic import Field
from pymongo import ReturnDocument

from src.common.types import Number
from src.pymodels import BaseModel
from src.request import ServerRequest


def currency_repository(request: ServerRequest) -> CurrencyRepository:
    return CurrencyRepository(request.app.state.mongo)


class Fields:
    user_id = "userId"
    BOUNTY_POINTS = "bountyPoints"
    ARMOURY_POINTS = "armouryPoints"
    PRESTIGE_POINTS = "prestigePoints"


class CurrenciesModel(BaseModel):
    prestige_points: int = Field(0, alias=Fields.PRESTIGE_POINTS)
    bounty_points: int = Field(0, alias=Fields.BOUNTY_POINTS)
    armoury_points: int = Field(0, alias=Fields.ARMOURY_POINTS)


class CurrencyRepository:
    def __init__(self, client):
        self.collection = client.database["currencyItems"]

    async def get_user(self, uid) -> CurrenciesModel:
        r = await self.collection.find_one(default := {Fields.user_id: uid})
        return CurrenciesModel.parse_obj(r or default)

    async def decr(self, uid: ObjectId, field: str, value: Number) -> CurrenciesModel:
        return await self.update_one(uid, {"$inc": {field: -value}})

    async def inc_value(self, uid: ObjectId, field: str, value: Number) -> CurrenciesModel:
        return await self.update_one(uid, {"$inc": {field: value}})

    async def inc_values(self, uid: ObjectId, fields: dict[str, Number]) -> CurrenciesModel:
        return await self.update_one(uid, {"$inc": fields})

    async def update_one(self, uid, update: dict) -> CurrenciesModel:
        r = await self.collection.find_one_and_update(
            {Fields.user_id: uid},
            update,
            upsert=True,
            return_document=ReturnDocument.AFTER
        )
        return CurrenciesModel.parse_obj(r)
