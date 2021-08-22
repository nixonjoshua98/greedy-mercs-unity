import re
import json
import bson

import datetime as dt

import urllib.parse

from fastapi import Request
from fastapi.routing import APIRoute
from fastapi.responses import Response
from fastapi.encoders import jsonable_encoder


def camel_to_snake(data: dict) -> dict:
    regex_pattern = re.compile(r'(?<!^)(?=[A-Z])')

    new_dict = dict()

    for k, v in data.items():
        new_key = regex_pattern.sub("_", k).lower()

        new_dict[new_key] = v

    return new_dict


class ServerResponse(Response):
    def __init__(self, content, *args, **kwargs):
        super(ServerResponse, self).__init__(json.dumps(content, default=self.default), *args, **kwargs)

    @staticmethod
    def default(o):
        if isinstance(o, bson.ObjectId):
            return str(o)

        elif isinstance(o, (dt.datetime, dt.datetime)):
            return int(o.timestamp() * 1000)

        return jsonable_encoder(o)


class ServerRequest(Request):

    def __init__(self, *args, **kwargs):
        super(ServerRequest, self).__init__(*args, **kwargs)

    async def json(self):
        if not hasattr(self, "_json"):
            body = await self.body()

            if self.method == "POST":
                decoded = urllib.parse.unquote(body.decode("UTF-8"))

                self._json = camel_to_snake(json.loads(decoded))

            else:
                self._json = camel_to_snake(json.loads(body))

        return self._json


class ServerRoute(APIRoute):

    def get_route_handler(self):
        original_route_handler = super().get_route_handler()

        async def custom_route_handler(request: Request):
            request = ServerRequest(request.scope, request.receive)

            return await original_route_handler(request)

        return custom_route_handler
