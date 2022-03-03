from __future__ import annotations

import contextlib as cl

from redis import Redis as RedisClient
from src.request import ServerRequest


def redis_client(request: ServerRequest) -> RedisClient:
    return request.app.state.redis_client


@cl.contextmanager
def execute_redis_pipeline(redis: RedisClient):
    pipeline = redis.pipeline()

    try:
        yield pipeline
    finally:
        pipeline.execute(raise_on_error=True)
