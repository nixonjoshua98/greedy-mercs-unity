from __future__ import annotations

from bson import ObjectId
from fastapi import Depends, HTTPException

from src.cache import MemoryCache, memory_cache
from src.routing import ServerRequest
from .session import Session


class AuthenticatedUser:
    def __init__(self, id_: ObjectId):
        self.id: ObjectId = id_


async def authenticated_user(request: ServerRequest, mem_cache: MemoryCache = Depends(memory_cache)):
    uid, did, sid = _grab_headers_from_request(request)

    # Throw an error if any required header is missing.
    if (not ObjectId.is_valid(uid)) or any(ele is None for ele in (uid, did, sid)):
        raise HTTPException(401, detail="Missing authentication")

    session: Session = mem_cache.get_session(uid := ObjectId(uid))

    """    
    if session is None or not session.is_valid():
        raise HTTPException(401, detail="Invalid session")
    """

    return AuthenticatedUser(id_=uid)


def _grab_headers_from_request(request: ServerRequest) -> tuple[str, str, str]:
    return request.headers.get("x-userid"), request.headers.get("x-deviceid"), request.headers.get("x-sessionid")
