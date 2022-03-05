import os
from typing import Union

from fastapi import FastAPI

from src import utils
from src.pymodels import ApplicationConfig


class Application(FastAPI):
    def __init__(self, *args, **kwargs):
        super(Application, self).__init__(*args, **kwargs)

        self.debug: bool = os.environ.get("DEBUG") is None
        self.config: ApplicationConfig = self._load_config()

    @staticmethod
    def _load_config() -> ApplicationConfig:
        return ApplicationConfig.parse_obj(utils.yaml_load(os.path.join(os.getcwd(), "config.yml")))

    def get_static_file(self, f: str) -> Union[dict, list]:
        return utils.load_static_data_file(f)
