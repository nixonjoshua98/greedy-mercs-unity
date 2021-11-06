from __future__ import annotations

from typing import Union

from src.pymodels import BaseDocument
from src.routing import ServerRequest


def inject_account_repo(request: ServerRequest) -> AccountsRepository:
    """Dependancy injection for the repository"""
    return AccountsRepository(request.app.state.mongo)


class AccountModel(BaseDocument):
    ...


class AccountsRepository:
    def __init__(self, client):
        self._col = client.db["userAccounts"]

    async def get_user(self, uid, did) -> Union[AccountModel, None]:
        r = await self._col.find_one({"_id": uid, "deviceId": did})

        return AccountModel.parse_obj(r) if r else None

    async def get_user_by_id(self, uid) -> Union[AccountModel, None]:
        r = await self._col.find_one({"_id": uid})

        return AccountModel.parse_obj(r) if r else None

    async def get_user_by_did(self, uid) -> Union[AccountModel, None]:
        r = await self._col.find_one({"deviceId": uid})

        return AccountModel.parse_obj(r) if r else None

    async def insert_new_user(self, data: dict) -> Union[AccountModel, None]:
        r = await self._col.insert_one(data)

        return await self.get_user_by_id(r.inserted_id)
