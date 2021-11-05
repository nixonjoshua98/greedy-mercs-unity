
from typing import Optional, TYPE_CHECKING

if TYPE_CHECKING:
    from .cache import MemoryCache


class PrefixedCache:
    def __init__(self, cache, prefix: str):
        self._prefix: str = prefix

        self._cache: MemoryCache = cache

    def get(self, uid) -> Optional[str]:
        return self._cache.get(f"{self._prefix}/{uid}")

    def set(self, uid, ses):
        self._cache.set(f"{self._prefix}/{uid}", ses)
