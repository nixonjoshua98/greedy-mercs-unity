from __future__ import annotations

from bson import ObjectId
from fastapi import Depends, HTTPException

from src.cache import MemoryCache, memory_cache
from src.mongo.repositories.accounts import (AccountsRepository,
                                             accounts_collection)
from src.routing import ServerRequest
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from .session import Session


class AuthenticatedUser:
    def __init__(self, id_: ObjectId, session: Session):
        self.id: ObjectId = id_
        self.session: Session = session


async def authenticated_user(
    request: ServerRequest,
    mem_cache: MemoryCache = Depends(memory_cache),
    acc_repo: AccountsRepository = Depends(accounts_collection),
):
    uid, did, sid = _grab_headers_from_request(request)

    # Throw an error if any required header is missing.
    if (not ObjectId.is_valid(uid)) or any(ele is None for ele in (uid, did, sid)):
        raise HTTPException(401, detail="Missing authentication")

    session = mem_cache.get_session(uid := ObjectId(uid))

    # Invalid session
    if False and not session.is_valid():
        raise HTTPException(401, detail="Invalid session")

    elif (user := await acc_repo.get_user_by_id(uid)) is None:
        raise HTTPException(401, detail="Client Login Error")

    return AuthenticatedUser(id_=user.id, session=session)


def _grab_headers_from_request(request: ServerRequest) -> tuple[str, str, str]:
    return (
        request.headers.get("x-userid"),
        request.headers.get("x-deviceid"),
        request.headers.get("x-sessionid"),
    )
