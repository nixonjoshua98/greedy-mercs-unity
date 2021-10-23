from typing import Any

VOID = "<VOID>"


class MemoryCache:
    def __init__(self):
        self.__internal_dict: dict = {}

    def get_value(self, key: str, default: Any = VOID) -> Any:
        if default != VOID:
            return self.__internal_dict.get(key, default)

        return self.__internal_dict[key]

    def set_value(self, key: str, value: Any):
        self.__internal_dict[key] = value
