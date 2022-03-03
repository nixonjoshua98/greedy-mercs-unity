import os
from typing import Union

from fastapi import FastAPI

from src import utils
from src.auth import AuthenticationService
from src.pymodels import ApplicationConfig


class Application(FastAPI):
    def __init__(self, *args, **kwargs):
        super(Application, self).__init__(*args, **kwargs)

        self._static_files: dict[str, Union[dict, list]] = dict()

        self.debug: bool = os.environ.get("DEBUG") is None
        self.config: ApplicationConfig = self._load_config()

    @staticmethod
    def _load_config() -> ApplicationConfig:
        d: dict = utils.yaml_load(os.path.join(os.getcwd(), "config.yml"))

        return ApplicationConfig.parse_obj(d)

    def get_static_file(self, f: str) -> Union[dict, list]:
        if not (d := self._static_files.get(f)):
            d = self._static_files[f] = utils.load_static_data_file(f)
        return d
