import dataclasses

import multipledispatch as md
from bson import ObjectId
from fastapi import Depends

from src.handlers.abc import BaseHandler, BaseResponse
from src.mongo.accounts import (AccountModel, AccountsRepository,
                                accounts_repository)
from src.request_models import LoginData


@dataclasses.dataclass()
class AccountCreationRequest:
    device_id: str


@dataclasses.dataclass()
class AccountCreationResponse(BaseResponse):
    user_id: ObjectId


class CreateAccountHandler(BaseHandler):
    def __init__(self, acc_repo: AccountsRepository = Depends(accounts_repository)):
        self.accounts_repo: AccountsRepository = acc_repo

    @md.dispatch(LoginData)
    async def handle(self, data: LoginData):
        user: AccountModel = await self.accounts_repo.insert_new_user(data.device_id)

        return AccountCreationResponse(user_id=user.id)

    @md.dispatch(AccountCreationRequest)
    async def handle(self, data: AccountCreationRequest):
        user: AccountModel = await self.accounts_repo.insert_new_user(data.device_id)

        return AccountCreationResponse(user_id=user.id)

