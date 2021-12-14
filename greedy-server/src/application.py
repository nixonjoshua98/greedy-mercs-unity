import os
from typing import Union

from cachetools import TTLCache
from cachetools import cached as cached_decorator
from fastapi import FastAPI

from src import utils


class Application(FastAPI):
    def __init__(self, *args, **kwargs):
        super(Application, self).__init__(*args, **kwargs)

        self.debug = os.environ.get("DEBUG", "0") == "1"
        self.config = utils.yaml_load(os.path.join(os.getcwd(), "config.yaml"))

    @cached_decorator(TTLCache(maxsize=1024, ttl=0))
    def get_static_file(self, f: str) -> Union[dict, list]:
        """Load a static data file and cache it"""
        return utils.load_static_data_file(f)
