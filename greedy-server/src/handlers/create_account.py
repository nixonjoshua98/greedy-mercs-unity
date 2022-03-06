import dataclasses

from fastapi import Depends

from src.handlers.abc import BaseHandler, BaseResponse
from src.mongo.repositories.accounts import (AccountModel, AccountsRepository,
                                             accounts_repository)
from src.request_models import LoginModel


@dataclasses.dataclass()
class AccountCreationResponse(BaseResponse):
    account: AccountModel


class CreateAccountHandler(BaseHandler):
    def __init__(self, acc_repo: AccountsRepository = Depends(accounts_repository)):
        self.accounts_repo: AccountsRepository = acc_repo

    async def handle(self, data: LoginModel):
        return await self._handle(data.device_id)

    async def _handle(self, device: str) -> AccountCreationResponse:
        user: AccountModel = await self.accounts_repo.insert_new_user(device)

        return AccountCreationResponse(account=user)

