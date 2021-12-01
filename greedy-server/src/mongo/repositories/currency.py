from __future__ import annotations

from pydantic import Field
from pymongo import ReturnDocument

from src.pymodels import BaseDocument
from src.routing import ServerRequest


def inject_currency_repository(request: ServerRequest) -> CurrencyRepository:
    return CurrencyRepository(request.app.state.mongo)


# = Field Keys = #


class Fields:
    BOUNTY_POINTS = "bountyPoints"
    ARMOURY_POINTS = "armouryPoints"
    PRESTIGE_POINTS = "prestigePoints"


# = Models = #


class CurrenciesModel(BaseDocument):
    prestige_points: int = Field(0, alias=Fields.PRESTIGE_POINTS)
    bounty_points: int = Field(0, alias=Fields.BOUNTY_POINTS)
    armoury_points: int = Field(0, alias=Fields.ARMOURY_POINTS)

    def to_client_dict(self):
        return self.dict(exclude={"id"})


# = Repository = #


class CurrencyRepository:
    def __init__(self, client):
        self.collection = client.db["currencyItems"]

    async def get_user(self, uid) -> CurrenciesModel:
        r = await self.collection.find_one({"_id": uid})

        return CurrenciesModel.parse_obj(r or {"_id": uid})

    async def update_one(self, uid, update: dict) -> CurrenciesModel:
        r = await self.collection.find_one_and_update(
            {"_id": uid}, update, upsert=True, return_document=ReturnDocument.AFTER
        )

        return CurrenciesModel.parse_obj(r)
