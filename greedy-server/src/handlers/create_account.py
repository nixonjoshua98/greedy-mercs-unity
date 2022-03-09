import dataclasses

from fastapi import Depends

from src.handlers.abc import BaseHandler, BaseResponse
from src.mongo.repositories.accounts import (AccountModel, AccountsRepository,
                                             accounts_repository)


@dataclasses.dataclass()
class AccountCreationRequest:
    device_id: str


@dataclasses.dataclass()
class AccountCreationResponse(BaseResponse):
    account: AccountModel


class CreateAccountHandler(BaseHandler):
    def __init__(self, acc_repo: AccountsRepository = Depends(accounts_repository)):
        self.accounts_repo: AccountsRepository = acc_repo

    async def handle(self, data: AccountCreationRequest):
        user: AccountModel = await self.accounts_repo.insert_new_user(data.device_id)

        return AccountCreationResponse(account=user)

