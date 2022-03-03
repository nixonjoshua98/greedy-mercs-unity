from __future__ import annotations

from redis import Redis as RedisClient
from src.request import ServerRequest


def redis_client(request: ServerRequest) -> RedisClient:
    return request.app.state.redis_client
