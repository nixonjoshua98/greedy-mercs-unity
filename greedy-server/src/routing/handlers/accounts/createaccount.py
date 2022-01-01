import dataclasses

from bson import ObjectId
from fastapi import Depends

from src.mongo.repositories.accounts import (AccountModel, AccountsRepository,
                                             accounts_repository)
from src.routing.handlers.abc import BaseHandler, BaseResponse


@dataclasses.dataclass()
class AccountCreationResponse(BaseResponse):
    user_id: ObjectId


class CreateAccountHandler(BaseHandler):
    def __init__(self, acc_repo=Depends(accounts_repository)):
        self.accounts_repo: AccountsRepository = acc_repo

    async def handle(self, device: str) -> AccountCreationResponse:
        user: AccountModel = await self.accounts_repo.insert_new_user(device)

        return AccountCreationResponse(user_id=user.id)

