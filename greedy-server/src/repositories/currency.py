from __future__ import annotations

from bson import ObjectId
from pydantic import Field
from pymongo import ReturnDocument

from src.common.types import NumberType
from src.request import ServerRequest
from src.shared_models import BaseModel


def get_currency_repository(request: ServerRequest) -> CurrencyRepository:
    return CurrencyRepository(request.app.state.mongo)


class Fields:
    user_id = "userId"
    diamonds = "diamonds"
    bounty_points = "bountyPoints"
    armoury_points = "armouryPoints"
    prestige_points = "prestigePoints"


class CurrenciesModel(BaseModel):
    user_id: ObjectId = Field(..., alias=Fields.user_id)
    diamonds: int = Field(0, alias=Fields.diamonds)
    prestige_points: int = Field(0, alias=Fields.prestige_points)
    bounty_points: int = Field(0, alias=Fields.bounty_points)
    armoury_points: int = Field(0, alias=Fields.armoury_points)


class CurrencyRepository:
    def __init__(self, client):
        self._items = client.database["currencyItems"]

    async def get_user(self, uid) -> CurrenciesModel:
        r = await self._items.find_one(default := {Fields.user_id: uid})
        return CurrenciesModel.parse_obj(r or default)

    async def decr(self, uid: ObjectId, field: str, value: NumberType) -> CurrenciesModel:
        return await self.incr(uid, field, -value)

    async def incr(self, uid: ObjectId, field: str, value: NumberType) -> CurrenciesModel:
        return await self.update_one(uid, {"$inc": {field: value}})

    async def inc_values(self, uid: ObjectId, fields: dict[str, NumberType]) -> CurrenciesModel:
        return await self.update_one(uid, {"$inc": fields})

    async def update_one(self, uid, update: dict) -> CurrenciesModel:
        r = await self._items.find_one_and_update(
            {Fields.user_id: uid},
            update,
            upsert=True,
            return_document=ReturnDocument.AFTER
        )
        return CurrenciesModel.parse_obj(r)
