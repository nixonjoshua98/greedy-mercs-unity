from __future__ import annotations

import datetime as dt
from typing import Optional

from bson import ObjectId
from fastapi import Depends, HTTPException

from src.cache import MemoryCache, memory_cache
from src.routing import ServerRequest

from .session import Session


class RequestContext:
    def __init__(self, uid: ObjectId):
        self.user_id: ObjectId = uid
        self.datetime: dt.datetime = dt.datetime.utcnow()
        self.prev_daily_reset: dt.datetime = _prev_daily_reset_datetime(self.datetime)

    @classmethod
    def from_session(cls, session: Session) -> RequestContext:
        return RequestContext(session.user_id)


async def request_context(
    request: ServerRequest, cache: MemoryCache = Depends(memory_cache)
) -> RequestContext:
    if (auth_key := _get_auth_from_request(request)) is None:
        raise HTTPException(401, detail="Unauthorized")

    elif (session := _get_session_from_cache(cache, auth_key)) is None:
        raise HTTPException(401, detail="Unauthorized")

    return RequestContext.from_session(session)


def _get_auth_from_request(request: ServerRequest) -> Optional[str]:
    return request.headers.get("authentication")


def _get_session_from_cache(cache: MemoryCache, key: str) -> Optional[Session]:
    return cache.get_session(key)


def _prev_daily_reset_datetime(now: dt.datetime) -> dt.datetime:
    reset_time = now.replace(hour=20, minute=0, second=0, microsecond=0)

    return reset_time - dt.timedelta(days=1) if now <= reset_time else reset_time
