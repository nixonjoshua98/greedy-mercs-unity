from typing import Any

from fastapi.responses import JSONResponse as _JSONResponse

from src import utils
from src.pymodels import BaseModel


class ServerResponse(_JSONResponse):
    def __init__(self, content: dict, *args, **kwargs):
        super(ServerResponse, self).__init__(content, *args, **kwargs)

    def render(self, content: Any) -> bytes:
        return utils.json_dumps(content, default=self._json_encoder).encode("utf-8")

    @staticmethod
    def _json_encoder(value: Any):
        if isinstance(value, BaseModel):
            return value.client_dict()

        return utils.default_json_encoder(value)
