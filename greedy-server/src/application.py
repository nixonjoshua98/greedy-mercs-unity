from typing import Union

from fastapi import FastAPI

from src import utils


class Application(FastAPI):
    def __init__(self, *args, **kwargs):
        super(Application, self).__init__(*args, **kwargs)

        self._static_files = {}

    def get_static_file(self, f: str) -> Union[dict, list]:
        return utils.load_static_data_file(f)
        if not (d := self._static_files.get(f, None)):
            d = self._static_files[f] = utils.load_static_data_file(f)

        return d
