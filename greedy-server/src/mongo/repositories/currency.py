from __future__ import annotations

from typing import Union

from bson import ObjectId
from pydantic import Field
from pymongo import ReturnDocument

from src.pymodels import BaseDocument
from src.routing import ServerRequest


def currency_repository(request: ServerRequest) -> CurrencyRepository:
    return CurrencyRepository(request.app.state.mongo)


class Fields:
    BOUNTY_POINTS = "bountyPoints"
    ARMOURY_POINTS = "armouryPoints"
    PRESTIGE_POINTS = "prestigePoints"


class CurrenciesModel(BaseDocument):
    prestige_points: int = Field(0, alias=Fields.PRESTIGE_POINTS)
    bounty_points: int = Field(0, alias=Fields.BOUNTY_POINTS)
    armoury_points: int = Field(0, alias=Fields.ARMOURY_POINTS)

    def client_dict(self):
        return self.dict(exclude={"id"})


class CurrencyRepository:
    def __init__(self, client):
        self.collection = client.database["currencyItems"]

    async def get_user(self, uid) -> CurrenciesModel:
        r = await self.collection.find_one({"_id": uid})

        return CurrenciesModel.parse_obj(r or {"_id": uid})

    async def inc_value(self, uid: ObjectId, field: str, value: Union[int, float]) -> CurrenciesModel:
        return await self.update_one(uid, {"$inc": {field: value}})

    async def inc_values(self, uid: ObjectId, fields: dict[str, Union[int, float]]) -> CurrenciesModel:
        return await self.update_one(uid, {"$inc": fields})

    async def update_one(self, uid, update: dict) -> CurrenciesModel:
        r = await self.collection.find_one_and_update(
            {"_id": uid}, update, upsert=True, return_document=ReturnDocument.AFTER
        )

        return CurrenciesModel.parse_obj(r)
