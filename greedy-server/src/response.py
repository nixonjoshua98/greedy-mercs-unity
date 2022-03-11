from typing import Any

from fastapi.responses import JSONResponse as _JSONResponse
from pydantic import BaseModel

from src import utils


class ServerResponse(_JSONResponse):
    def __init__(self, content: Any, *args, **kwargs):
        super(ServerResponse, self).__init__(content, *args, **kwargs)

    def render(self, content: Any) -> bytes:
        if isinstance(content, BaseModel):
            content = content.dict()

        dumped = utils.json_dumps(
            content,
            default=utils.default_json_encoder
        )

        return dumped.encode("utf-8")


class EncryptedServerResponse(ServerResponse):
    ...
