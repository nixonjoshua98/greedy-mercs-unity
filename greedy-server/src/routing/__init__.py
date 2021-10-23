import re
import json
import bson

import urllib.parse
from fastapi import Request as _Request

import datetime as dt

from fastapi.responses import Response as _Response
from fastapi.encoders import jsonable_encoder as _jsonable_encoder

from fastapi.routing import APIRoute as _APIRoute, APIRouter as _APIRouter


def _camel_to_snake(data: dict) -> dict:
    regex_pattern = re.compile(r'(?<!^)(?=[A-Z])')

    new_dict = dict()

    for k, v in data.items():
        new_key = regex_pattern.sub("_", k).lower()

        new_dict[new_key] = v

    return new_dict


class ServerRequest(_Request):

    async def json(self):
        if not hasattr(self, "_json"):
            body = await self.body()

            if self.method == "POST":
                decoded = urllib.parse.unquote(body.decode("UTF-8"))

                self._json = _camel_to_snake(json.loads(decoded))

            else:
                self._json = _camel_to_snake(json.loads(body))

        return self._json


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

        return _jsonable_encoder(o)


class ServerRoute(_APIRoute):

    def get_route_handler(self):
        original_route_handler = super().get_route_handler()

        async def custom_route_handler(request: _Request):
            request = ServerRequest(request.scope, request.receive)

            return await original_route_handler(request)

        return custom_route_handler


class APIRouter(_APIRouter):
    def __init__(self, *, route_class=ServerRoute, **kwargs):
        super(APIRouter, self).__init__(route_class=route_class, **kwargs)
