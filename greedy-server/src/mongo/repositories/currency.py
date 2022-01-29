from __future__ import annotations

from typing import Union

from bson import ObjectId
from pydantic import Field
from pymongo import ReturnDocument

from src.pymodels import BaseDocument
from src.routing import ServerRequest


def currency_repository(request: ServerRequest) -> CurrencyRepository:
    return CurrencyRepository(request.app.state.mongo)


class CurrenciesModel(BaseDocument):
    class Aliases:
        USER_ID = "_id"
        BOUNTY_POINTS = "bountyPoints"
        ARMOURY_POINTS = "armouryPoints"
        PRESTIGE_POINTS = "prestigePoints"

    prestige_points: int = Field(0, alias=Aliases.PRESTIGE_POINTS)
    bounty_points: int = Field(0, alias=Aliases.BOUNTY_POINTS)
    armoury_points: int = Field(0, alias=Aliases.ARMOURY_POINTS)

    def client_dict(self):
        return self.dict(exclude={"id"})


class CurrencyRepository:
    def __init__(self, client):
        self.collection = client.database["currencyItems"]

    async def get_user(self, uid) -> CurrenciesModel:
        r = await self.collection.find_one({
            CurrenciesModel.Aliases.USER_ID: uid
        })

        return CurrenciesModel.parse_obj(r or {CurrenciesModel.Aliases.USER_ID: uid})

    async def inc_value(self, uid: ObjectId, field: str, value: Union[int, float]) -> CurrenciesModel:
        return await self.update_one(uid, {"$inc": {field: value}})

    async def inc_values(self, uid: ObjectId, fields: dict[str, Union[int, float]]) -> CurrenciesModel:
        return await self.update_one(uid, {"$inc": fields})

    async def update_one(self, uid, update: dict) -> CurrenciesModel:
        r = await self.collection.find_one_and_update({
            CurrenciesModel.Aliases.USER_ID: uid},
            update,
            upsert=True,
            return_document=ReturnDocument.AFTER
        )

        return CurrenciesModel.parse_obj(r)
