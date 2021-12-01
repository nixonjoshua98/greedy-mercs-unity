import datetime as dt
import json
from typing import Any

import bson
from fastapi.encoders import jsonable_encoder as _jsonable_encoder
from fastapi.responses import JSONResponse as _JSONResponse


class ServerResponse(_JSONResponse):
    def __init__(self, content: dict, *args, **kwargs):
        super(ServerResponse, self).__init__(content, *args, **kwargs)

    def render(self, content: Any) -> bytes:
        return json.dumps(
            content,
            ensure_ascii=False,
            allow_nan=False,
            default=self.default_json_dump
        ).encode("utf-8")

    @staticmethod
    def default_json_dump(o):
        if isinstance(o, bson.ObjectId):
            return str(o)

        elif isinstance(o, (dt.datetime, dt.datetime)):
            return int(o.timestamp() * 1000)

        return _jsonable_encoder(o)
