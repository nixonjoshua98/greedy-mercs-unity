from bson import ObjectId
from fastapi import Depends, HTTPException, Request

from src.cache import MemoryCache, inject_memory_cache
from src.pymodels import BaseModel


async def inject_user(
        request: Request,
        mem_cache: MemoryCache = Depends(inject_memory_cache)
):
    uid = request.headers.get("x-userid")
    did = request.headers.get("x-deviceid")
    sid = request.headers.get("x-sessionid")

    # Throw an error if any required header is missing
    if (not ObjectId.is_valid(uid)) or any(ele is None for ele in (uid, did, sid)):
        raise HTTPException(401, detail="Missing authentication credentials")

    elif mem_cache.get_value(f"session/{uid}", None) != sid:
        raise HTTPException(401, detail="Invalid session")

    return AuthenticatedUser(id=ObjectId(uid))


class AuthenticatedUser(BaseModel):
    id: ObjectId
