from typing import Any

from fastapi.responses import JSONResponse as _JSONResponse

from src import utils


class ServerResponse(_JSONResponse):
    def __init__(self, content: dict, *args, **kwargs):
        super(ServerResponse, self).__init__(content, *args, **kwargs)

    def render(self, content: Any) -> bytes:
        return utils.json_dump(content).encode("utf-8")
