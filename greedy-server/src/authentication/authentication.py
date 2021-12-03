from bson import ObjectId
from fastapi import Depends, HTTPException

from src.cache import MemoryCache, inject_memory_cache
from src.mongo.repositories.accounts import (AccountsRepository,
                                             inject_account_repo)
from src.routing import ServerRequest


class AuthenticatedUser:
    id: ObjectId

    def __init__(self, id_: ObjectId):
        self.id = id_


async def authenticated_user(
    request: ServerRequest,
    mem_cache: MemoryCache = Depends(inject_memory_cache),
    acc_repo: AccountsRepository = Depends(inject_account_repo),
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

    return AuthenticatedUser(id_=user.id)


def _grab_headers_from_request(request: ServerRequest) -> tuple[str, str, str]:
    return (
        request.headers.get("x-userid"),
        request.headers.get("x-deviceid"),
        request.headers.get("x-sessionid"),
    )
