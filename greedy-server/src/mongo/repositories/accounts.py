from __future__ import annotations

from typing import Optional

from src.pymodels import BaseDocument
from src.routing import ServerRequest


def accounts_repository(request: ServerRequest) -> AccountsRepository:
    return AccountsRepository(request.app.state.mongo)


class AccountModel(BaseDocument):

    class Alias:
        USER_ID = "_id"
        DEVICE_ID = "deviceId"


class AccountsRepository:
    def __init__(self, client):
        self._col = client.database["userAccounts"]

    async def get_user(self, uid) -> Optional[AccountModel]:
        r = await self._col.find_one({
            AccountModel.Alias.USER_ID: uid
        })

        return AccountModel.parse_obj(r) if r else None

    async def get_user_by_did(self, uid) -> Optional[AccountModel]:
        r = await self._col.find_one({
            AccountModel.Alias.DEVICE_ID: uid
        })

        return AccountModel.parse_obj(r) if r else None

    async def insert_new_user(self, device_id: str) -> AccountModel:
        r = await self._col.insert_one({
            AccountModel.Alias.DEVICE_ID: device_id
        })

        return await self.get_user(r.inserted_id)
