import json
import bson

import datetime as dt

from fastapi.responses import Response as _Response
from fastapi.encoders import jsonable_encoder


class ServerResponse(_Response):
    def __init__(self, content: dict, *args, **kwargs):
        super(ServerResponse, self).__init__(self._dump_content(content), *args, **kwargs)

    def _dump_content(self, content):
        if isinstance(content, str):
            return content

        return json.dumps(content, default=self.jsonable_encoder)

    @staticmethod
    def jsonable_encoder(o):
        if isinstance(o, bson.ObjectId):
            return str(o)

        elif isinstance(o, (dt.datetime, dt.datetime)):
            return int(o.timestamp() * 1000)

        return jsonable_encoder(o)
