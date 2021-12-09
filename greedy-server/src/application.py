from typing import Union

from cachetools import TTLCache
from fastapi import FastAPI

from src import utils


class Application(FastAPI):
    def __init__(self, *args, **kwargs):
        super(Application, self).__init__(*args, **kwargs)

        self._static_files = TTLCache(maxsize=1024, ttl=0)

    def get_static_file(self, f: str) -> Union[dict, list]:
        """Fetch a static data file from cache or file"""

        if not (d := self._static_files.get(f)):
            d = self._static_files[f] = utils.load_static_data_file(f)

        return d
