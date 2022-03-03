from __future__ import annotations

from redis import Redis as RedisClient

from src.request import ServerRequest


def redis_client(request: ServerRequest) -> RedisClient:
    return request.app.state.redis_client


class RedisPrefix:
    USER_2_SESSION = "User-Session"
    SESSION_2_USER = "Session-User"
