from typing import Optional

from fastapi import Depends

from src.common.constants import StatusCodes
from src.dependencies import get_auth_token_header, get_device_id_header
from src.exceptions import ServerException
from src.mongo.accounts import (AccountModel, AccountsRepository,
                                accounts_repository)

from .context import AuthenticatedRequestContext


async def get_authenticated_context(
    device_id: str = Depends(get_device_id_header),
    auth_key: str = Depends(get_auth_token_header),
    accounts: AccountsRepository = Depends(accounts_repository)
):
    handler = AuthenticationHandler(accounts)

    return await handler.handle(auth_key, device_id)


class AuthenticationHandler:
    def __init__(self, accounts: AccountsRepository):
        self._accounts: AccountsRepository = accounts

    async def handle(self, auth_key: str, device_id: str) -> AuthenticatedRequestContext:

        if auth_key is None or device_id is None:
            raise ServerException(401, "Unauthorized")

        account: Optional[AccountModel] = await self._accounts.get_user_by_session(auth_key)

        if account is None:
            raise ServerException(StatusCodes.INVALIDATE_CLIENT, "Unauthorized")

        elif device_id != account.session.device_id:
            raise ServerException(StatusCodes.INVALIDATE_CLIENT, "Unauthorized")

        return AuthenticatedRequestContext(uid=account.id)
