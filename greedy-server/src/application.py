from typing import Union

from cachetools import TTLCache, cached as cached_decorator
from fastapi import FastAPI
import functools as ft
from src import utils
import yaml
import os


class Application(FastAPI):

    @ft.cached_property
    def config(self) -> dict:
        f: str = os.path.join(os.getcwd(), "config.yaml")
        with open(f) as fh:
            return yaml.safe_load(fh)

    @cached_decorator(TTLCache(maxsize=1024, ttl=0))
    def get_static_file(self, f: str) -> Union[dict, list]:
        """Load a static data file and cache it"""
        return utils.load_static_data_file(f)
