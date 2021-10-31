from bson import ObjectId
from fastapi import Depends, HTTPException

from src import logger
from src.cache import MemoryCache, inject_memory_cache
from src.pymodels import BaseModel
from src.routing import ServerRequest
from src.mongo.repositories.accounts import inject_account_repo, AccountsRepository, AccountModel


class AuthenticatedUser(BaseModel):
    id: ObjectId


async def inject_authenticated_user(
        request: ServerRequest,
        mem_cache: MemoryCache = Depends(inject_memory_cache),
        acc_repo: AccountsRepository = Depends(inject_account_repo)
):
    uid, did, sid = _grab_headers_from_request(request)

    # Throw an error if any required header is missing.
    if (not ObjectId.is_valid(uid)) or any(ele is None for ele in (uid, did, sid)):
        raise HTTPException(401, detail="Missing authentication")

    # Invalid session
    elif False and mem_cache.get_session(uid) != sid:
        raise HTTPException(401, detail="Invalid session")

    # Throw an error if the user suddenly is on a different device now
    # Logging in will change the registered deviceId, so if this error is thrown
    # it's most likely due to someone replicating the API (or my mistake)
    elif (user := await acc_repo.get_user(ObjectId(uid), did)) is None:
        raise HTTPException(400, detail="Error")

    return AuthenticatedUser(id=user.id)


def _grab_headers_from_request(request: ServerRequest) -> tuple[str, str, str]:
    return (
        request.headers.get("x-userid"),
        request.headers.get("x-deviceid"),
        request.headers.get("x-sessionid"),
    )
