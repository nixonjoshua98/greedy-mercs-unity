from fastapi import Request

from .prefixedcache import PrefixedCache


def inject_memory_cache(request: Request):
    return request.app.state.memory_cache


class MemoryCache:
    def __init__(self):
        self.__internal_dict: dict = {}

        self.sessions = PrefixedCache(self, "sessions")

    def get(self, key, default=None):
        return self.__internal_dict.get(key, default)

    def set(self, key, value):
        self.__internal_dict[key] = value
