import functools as ft
import os
from typing import Union

import yaml
from cachetools import TTLCache
from cachetools import cached as cached_decorator
from fastapi import FastAPI

from src import utils


class Application(FastAPI):

    def __init__(self, *args, **kwargs):
        super(Application, self).__init__(*args, **kwargs)

        self.debug = os.environ.get("DEBUG", "0") == "1"

    @ft.cached_property
    def config(self) -> dict:
        f: str = os.path.join(os.getcwd(), "config.yaml")
        with open(f) as fh:
            return yaml.safe_load(fh)

    @cached_decorator(TTLCache(maxsize=1024, ttl=0))
    def get_static_file(self, f: str) -> Union[dict, list]:
        """Load a static data file and cache it"""
        return utils.load_static_data_file(f)
