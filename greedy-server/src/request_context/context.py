from __future__ import annotations

import datetime as dt
from typing import Optional

from bson import ObjectId
from fastapi import Depends, HTTPException

from src.cache import MemoryCache, memory_cache
from src.routing import ServerRequest

from .session import Session


class RequestContext:
    def __init__(self):
        self.datetime: dt.datetime = dt.datetime.utcnow()

        self.prev_daily_reset: dt.datetime = _prev_daily_reset_datetime(self.datetime)
        self.next_daily_reset: dt.datetime = self.prev_daily_reset + dt.timedelta(days=1)


class AuthenticatedRequestContext(RequestContext):
    def __init__(self, uid: ObjectId):
        super(AuthenticatedRequestContext, self).__init__()

        self.user_id: ObjectId = uid


async def authenticated_context(
        request: ServerRequest, cache: MemoryCache = Depends(memory_cache)
) -> AuthenticatedRequestContext:

    if (auth_key := request.headers.get("authentication")) is None:
        raise HTTPException(401, detail="Unauthorized")

    elif (session := _get_session_from_cache(cache, auth_key)) is None:
        raise HTTPException(401, detail="Unauthorized")

    return AuthenticatedRequestContext(uid=session.user_id)


def _get_session_from_cache(cache: MemoryCache, key: str) -> Optional[Session]:
    return cache.get_session(key)


def _prev_daily_reset_datetime(now: dt.datetime) -> dt.datetime:
    reset_time = now.replace(hour=20, minute=0, second=0, microsecond=0)

    return reset_time - dt.timedelta(days=1) if now <= reset_time else reset_time
