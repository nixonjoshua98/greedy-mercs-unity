import os
from typing import Union

from cachetools import TTLCache
from fastapi import FastAPI

from src import utils
from src.pymodels import ApplicationConfig


class Application(FastAPI):
    def __init__(self, *args, **kwargs):
        super(Application, self).__init__(*args, **kwargs)

        self.static_files = TTLCache(1024, 0)

        self.debug: bool = os.environ.get("DEBUG", "0") == "1"
        self.config: ApplicationConfig = self._load_config()

    @staticmethod
    def _load_config() -> ApplicationConfig:
        d: dict = utils.yaml_load(os.path.join(os.getcwd(), "config.yaml"))

        return ApplicationConfig.parse_obj(d)

    def get_static_file(self, f: str) -> Union[dict, list]:
        if not (d := self.static_files.get(f)):
            d = self.static_files[f] = utils.load_static_data_file(f)
        return d
