from typing import Any

VOID = "<VOID>"


class MemoryCache:
    def __init__(self):
        self.__internal_dict: dict = {}

    def get_session_id(self, uid): return self.get_value(f"session/{uid}", None)
    def set_session_id(self, uid, session): return self.set_value(f"session/{uid}", session)

    def get_value(self, key: str, default: Any = VOID) -> Any:
        if default != VOID:
            return self.__internal_dict.get(key, default)

        return self.__internal_dict[key]

    def set_value(self, key: str, value: Any):
        self.__internal_dict[key] = value
