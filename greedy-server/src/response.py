from typing import Any

from fastapi.responses import JSONResponse as _Response
from pydantic import BaseModel

from src import utils
from src.common import aes


class ServerResponse(_Response):
    def __init__(self, content: Any, *args, **kwargs):
        super(ServerResponse, self).__init__(content, *args, **kwargs)

    def render(self, content: Any) -> bytes:
        return self.prepare_json(content).encode("utf-8")

    @classmethod
    def prepare_json(cls, content: Any) -> str:
        if isinstance(content, BaseModel):
            content = content.dict()

        return utils.json_dumps(content, default=utils.default_json_encoder)


class EncryptedServerResponse(ServerResponse):
    def __init__(self, content: Any, *args, **kwargs):
        headers = {"Response-Encrypted": "true", **kwargs.pop("headers", {})}

        super(EncryptedServerResponse, self).__init__(content, *args, headers=headers, **kwargs)

    def render(self, content: Any) -> bytes:
        return self._encrypt_body(content)

    def _encrypt_body(self, content: Any):
        json: str = self.prepare_json(content)

        return aes.encrypt(json).encode("utf-8")
