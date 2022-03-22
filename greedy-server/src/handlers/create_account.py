

import multipledispatch as md
from bson import ObjectId
from fastapi import Depends

from src.exceptions import HandlerException
from src.repositories.accounts import (AccountModel, AccountsRepository,
                                       get_accounts_repository)
from src.request_models import LoginRequestModel
from src.shared_models import BaseModel


class AccountCreationRequest:
    device_id: str


class AccountCreationResponse(BaseModel):
    user_id: ObjectId


class CreateAccountHandler:
    def __init__(self, acc_repo: AccountsRepository = Depends(get_accounts_repository)):
        self.accounts_repo: AccountsRepository = acc_repo

    @md.dispatch(LoginRequestModel)
    async def handle(self, data: LoginRequestModel):
        user: AccountModel = await self.accounts_repo.insert_new_user(data.device_id)

        return AccountCreationResponse(user_id=user.id)

    @md.dispatch(AccountCreationRequest)
    async def handle(self, data: AccountCreationRequest):
        user: AccountModel = await self.accounts_repo.insert_new_user(data.device_id)

        return AccountCreationResponse(user_id=user.id)

