from typing import Any

from fastapi.responses import JSONResponse as _JSONResponse

from src import utils
from src.pymodels import BaseModel


class ServerResponse(_JSONResponse):
    def __init__(self, content: Any, *args, **kwargs):
        super(ServerResponse, self).__init__(content, *args, **kwargs)

    def render(self, content: Any) -> bytes:
        if isinstance(content, BaseModel):
            content = content.client_dict()
        elif not isinstance(content, (dict, str)):
            raise TypeError(f"Attempted to return response type '{type(content)}'")

        return utils.json_dumps(content, default=self._json_default).encode("utf-8")

    @staticmethod
    def _json_default(value: Any):
        if isinstance(value, BaseModel):
            return value.client_dict()

        return utils.default_json_encoder(value)


class EncryptedServerResponse(ServerResponse):
    ...
