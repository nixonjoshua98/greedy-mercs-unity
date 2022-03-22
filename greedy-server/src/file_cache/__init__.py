import datetime as dt
from dataclasses import dataclass
from typing import Union

from src import utils
from src.request import ServerRequest


def get_static_files_cache(request: ServerRequest):
    return request.app.state.static_files_cache


@dataclass()
class StaticFileCacheObject:
    file_name: str
    last_loaded: dt.datetime
    data: Union[dict, list]


M15 = 60 * 15  # Reload in 15 minute intervals


class StaticFilesCache:
    _default_interval: int = M15

    def __init__(self):
        self._cache: dict[str, StaticFileCacheObject] = dict()

    def load_file(self, fp: str) -> Union[dict, list]:
        return self._cached_or_reload(fp)

    def load_quests(self) -> dict: return self._cached_or_reload("server/quests.json5")
    def load_mercs(self) -> dict: return self._cached_or_reload("mercs.json5")
    def load_artefacts(self) -> list: return self._cached_or_reload("artefacts.json")
    def load_bounties(self) -> dict: return self._cached_or_reload("bounties.json")
    def load_armoury(self) -> list: return self._cached_or_reload("armoury.json")

    def _cached_or_reload(self, fp: str):
        if fp not in self._cache:
            self._cache[fp] = self._create_new_cache_item(fp)

        elif self._is_item_outdated(self._cache[fp]):
            self._update_cached_item(self._cache[fp])

        return self._cache[fp].data

    @staticmethod
    def _create_new_cache_item(fp: str) -> StaticFileCacheObject:
        return StaticFileCacheObject(fp, dt.datetime.utcnow(), utils.load_static_data_file(fp))

    @staticmethod
    def _update_cached_item(obj: StaticFileCacheObject):
        obj.last_loaded = dt.datetime.utcnow()
        obj.data = utils.load_static_data_file(obj.file_name)

    def _is_item_outdated(self, obj: StaticFileCacheObject, *, interval: float = None) -> bool:
        """
        Check if the item is out of date and should be re-loaded from disk.

        Instead of having a TTL for cached items we have an interval (e.g items are refreshed at :15, :30 instead
        of at random times)

        :param obj: Cached item object

        :return:
            Boolean on if the cached item is valid
        """
        interval = interval or self._default_interval

        current_interval = int(dt.datetime.utcnow().timestamp() / interval)
        last_load_interval = int(obj.last_loaded.timestamp() / interval)

        return current_interval != last_load_interval
