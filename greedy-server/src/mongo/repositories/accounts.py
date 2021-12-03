from __future__ import annotations

from typing import Optional

from src.pymodels import BaseDocument
from src.routing import ServerRequest


def accounts_collection(request: ServerRequest) -> AccountsRepository:
    return AccountsRepository(request.app.state.mongo)


class AccountModel(BaseDocument):
    ...


class AccountsRepository:
    def __init__(self, client):
        self._col = client.db["userAccounts"]

    async def get_user_by_id(self, uid) -> Optional[AccountModel]:
        r = await self._col.find_one({"_id": uid})

        return AccountModel.parse_obj(r) if r else None

    async def get_user_by_did(self, uid) -> Optional[AccountModel]:
        r = await self._col.find_one({"deviceId": uid})

        return AccountModel.parse_obj(r) if r else None

    async def insert_new_user(self, device_id: str) -> Optional[AccountModel]:
        r = await self._col.insert_one({"deviceId": device_id})

        return await self.get_user_by_id(r.inserted_id)
