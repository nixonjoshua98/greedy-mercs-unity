from __future__ import annotations

from pymongo import ReturnDocument
from pydantic import Field

from src.routing import ServerRequest

from src.pymodels import BaseDocument


def inject_currencies_repository(request: ServerRequest) -> CurrenciesRepository:
    """ Used to inject a repository instance. """
    return CurrenciesRepository(request.app.state.mongo)


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

    def response_dict(self):
        return self.dict(exclude={"id"})


# = Repository = #


class CurrenciesRepository:
    def __init__(self, client):
        db = client.get_default_database()

        self.collection = db["currencyItems"]

    async def get_user(self, uid) -> CurrenciesModel:
        r = await self.collection.find_one({"_id": uid})

        return CurrenciesModel.parse_obj(r or {"_id": uid})

    async def update_one(self, uid, update: dict) -> CurrenciesModel:
        r = await self.collection.find_one_and_update(
            {"_id": uid}, update,
            upsert=True, return_document=ReturnDocument.AFTER
        )

        return CurrenciesModel.parse_obj(r)
