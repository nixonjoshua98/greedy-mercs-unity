from __future__ import annotations

from bson import ObjectId
from pydantic import Field
from pymongo import ReturnDocument

from src.pymodels import BaseDocument
from src.request import ServerRequest
from src.types import Number


def currency_repository(request: ServerRequest) -> CurrencyRepository:
    return CurrencyRepository(request.app.state.mongo)


class Fields:
    BOUNTY_POINTS = "bountyPoints"
    ARMOURY_POINTS = "armouryPoints"
    PRESTIGE_POINTS = "prestigePoints"


class CurrenciesModel(BaseDocument):

    class Aliases:
        bounty_points = "bountyPoints"
        armoury_points = "armouryPoints"
        prestige_points = "prestigePoints"

    prestige_points: int = Field(0, alias=Aliases.prestige_points)
    bounty_points: int = Field(0, alias=Aliases.bounty_points)
    armoury_points: int = Field(0, alias=Aliases.armoury_points)

    def client_dict(self):
        return self.dict(exclude={"id"})


class CurrencyRepository:
    def __init__(self, client):
        self.collection = client.database["currencyItems"]

    async def get_user(self, uid) -> CurrenciesModel:
        r = await self.collection.find_one({"_id": uid})
        return CurrenciesModel.parse_obj(r or {"_id": uid})

    async def decr(self, uid: ObjectId, field: str, value: Number) -> CurrenciesModel:
        return await self.update_one(uid, {"$inc": {field: -value}})

    async def inc_value(self, uid: ObjectId, field: str, value: Number) -> CurrenciesModel:
        return await self.update_one(uid, {"$inc": {field: value}})

    async def inc_values(self, uid: ObjectId, fields: dict[str, Number]) -> CurrenciesModel:
        return await self.update_one(uid, {"$inc": fields})

    async def update_one(self, uid, update: dict) -> CurrenciesModel:
        r = await self.collection.find_one_and_update(
            {"_id": uid}, update, upsert=True, return_document=ReturnDocument.AFTER
        )

        return CurrenciesModel.parse_obj(r)
