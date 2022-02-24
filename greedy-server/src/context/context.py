from __future__ import annotations

import datetime as dt
from typing import Optional

from bson import ObjectId
from fastapi import Depends, HTTPException

from src.cache import MemoryCache, memory_cache
from src.routing import ServerRequest


class RequestContext:
    def __init__(self):
        self.datetime: dt.datetime = dt.datetime.utcnow()
        self.prev_daily_reset: dt.datetime = _prev_daily_reset_datetime(self.datetime)
        self.next_daily_reset: dt.datetime = self.prev_daily_reset + dt.timedelta(days=1)


class AuthenticatedRequestContext(RequestContext):
    def __init__(self, uid: Optional[ObjectId]):
        super(AuthenticatedRequestContext, self).__init__()

        self.user_id: Optional[ObjectId] = uid


async def inject_authenticated_context(
    request: ServerRequest,
    cache: MemoryCache = Depends(memory_cache)
) -> AuthenticatedRequestContext:
    """
    Inject an 'AuthenticatedRequestContext' context (Forced endpoint to be authenticated) or throw an exception
    """
    key: Optional[str] = request.headers.get("authentication")

    # Header was not provided or session was not found
    if key is None or (session := cache.get_session(key) is None):
        raise HTTPException(401, detail="Unauthorized")

    return AuthenticatedRequestContext(uid=session.user_id)


def _prev_daily_reset_datetime(now: dt.datetime) -> dt.datetime:
    reset_time = now.replace(hour=20, minute=0, second=0, microsecond=0)

    return reset_time - dt.timedelta(days=1) if now <= reset_time else reset_time
