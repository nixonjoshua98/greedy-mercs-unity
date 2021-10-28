from fastapi import Request


def inject_memory_cache(request: Request):
    return request.app.state.memory_cache


class MemoryCache:
    def __init__(self):
        self.__internal_dict: dict = {}

    def set_session(self, uid, value):
        self.__internal_dict[f"session/{uid}"] = value

    def get_session(self, uid):
        return self.__internal_dict.get(f"session/{uid}", None)
