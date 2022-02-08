from __future__ import annotations

from typing import Union

from bson import ObjectId
from pydantic import Field
from pymongo import ReturnDocument

from src.pymodels import BaseModel
from src.routing import ServerRequest


def currency_repository(request: ServerRequest) -> CurrencyRepository:
    return CurrencyRepository(request.app.state.mongo)


class CurrenciesModel(BaseModel):

    class Aliases:
        USER_ID = "_id"
        BOUNTY_POINTS = "bountyPoints"
        ARMOURY_POINTS = "armouryPoints"
        PRESTIGE_POINTS = "prestigePoints"

    prestige_points: int = Field(0, alias=Aliases.PRESTIGE_POINTS)
    bounty_points: int = Field(0, alias=Aliases.BOUNTY_POINTS)
    armoury_points: int = Field(0, alias=Aliases.ARMOURY_POINTS)


class CurrencyRepository:
    def __init__(self, client):
        self.currencies = client.database["currencyItems"]

    async def get_user(self, uid) -> CurrenciesModel:
        """
        Fetch a users' currencies
        :param uid: User id
        :return:
            Model representing their currencies
        """
        r = await self.currencies.find_one({CurrenciesModel.Aliases.USER_ID: uid})

        return CurrenciesModel.parse_obj(r) if r else CurrenciesModel()

    async def inc_value(self, uid: ObjectId, field: str, value: Union[int, float]) -> CurrenciesModel:
        """
        Increment (or decrement) a single field
        :param uid: User id
        :param field: Mongo field name
        :param value: Value to increment by
        :return:
            Updated user currency model
        """
        return await self._update_one(uid, {"$inc": {field: value}})

    async def inc_values(self, uid: ObjectId, fields: dict[str, Union[int, float]]) -> CurrenciesModel:
        """
        Increment (or decrement) multiple fields
        :param uid: User id
        :param fields: Mongo field names and values to increment
        :return:
            Updated user currency model
        """
        return await self._update_one(uid, {"$inc": fields})

    # = Internal Methods = #

    async def _update_one(self, uid, update: dict) -> CurrenciesModel:
        """
        [Internal] Run an update on a single document then return the updated document as the model
        :param uid: User id
        :param update: Mongo update query
        :return:
            Updated user document model
        """
        r = await self.currencies.find_one_and_update({
            CurrenciesModel.Aliases.USER_ID: uid}, update,
            upsert=True, return_document=ReturnDocument.AFTER
        )

        return CurrenciesModel.parse_obj(r)
