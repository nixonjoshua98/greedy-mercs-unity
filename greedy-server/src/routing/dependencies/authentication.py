from bson import ObjectId
from fastapi import Depends, HTTPException

from src import logger
from src.cache import MemoryCache, inject_memory_cache
from src.pymodels import BaseModel
from src.routing import ServerRequest


class AuthenticatedUser(BaseModel):
    id: ObjectId


async def inject_authenticated_user(
    request: ServerRequest, mem_cache: MemoryCache = Depends(inject_memory_cache)
):
    uid, did, sid = _grab_headers_from_request(request)

    # Throw an error if any required header is missing
    if (not ObjectId.is_valid(uid)) or any(ele is None for ele in (uid, did, sid)):
        raise HTTPException(401, detail="Missing authentication credentials")

    """    elif mem_cache.get_session(uid) != sid:
        raise HTTPException(401, detail="Invalid session")"""

    # Cache the user for later dependancies for this request
    return AuthenticatedUser(id=ObjectId(uid))


def _grab_headers_from_request(request: ServerRequest) -> tuple[str, str, str]:
    return (
        request.headers.get("x-userid"),
        request.headers.get("x-deviceid"),
        request.headers.get("x-sessionid"),
    )
