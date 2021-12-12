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
    if (auth_header := request.headers.get("authentication")) is None:
        raise HTTPException(401, detail="Unauthorized")

    session: Session = mem_cache.get_session(auth_header)

    if session is None:
        raise HTTPException(401, detail="Unauthorized")

    return AuthenticatedUser(session.user_id)
