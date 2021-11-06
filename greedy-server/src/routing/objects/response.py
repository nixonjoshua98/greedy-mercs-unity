import datetime as dt
import json
from typing import Any

import bson
from fastapi.encoders import jsonable_encoder as _jsonable_encoder
from fastapi.responses import Response as _Response


class ServerResponse(_Response):
    def __init__(self, content: dict, *args, **kwargs):
        super(ServerResponse, self).__init__(content, *args, **kwargs)

    def render(self, content: Any) -> bytes:

        def _render():
            if isinstance(content, str):
                return content

            return json.dumps(content, default=self.jsonable_encoder)

        return super(ServerResponse, self).render(_render())

    @staticmethod
    def jsonable_encoder(o):
        if isinstance(o, bson.ObjectId):
            return str(o)

        elif isinstance(o, (dt.datetime, dt.datetime)):
            return int(o.timestamp() * 1000)

        return _jsonable_encoder(o)
