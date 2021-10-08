from fastapi import Request
from fastapi.routing import APIRoute as _APIRoute, APIRouter as _APIRouter

from .request import ServerRequest


class ServerRoute(_APIRoute):

    def get_route_handler(self):
        original_route_handler = super().get_route_handler()

        async def custom_route_handler(request: Request):
            request = ServerRequest(request.scope, request.receive)

            return await original_route_handler(request)

        return custom_route_handler


class APIRouter(_APIRouter):
    def __init__(self, *, route_class=ServerRoute, **kwargs):
        super(APIRouter, self).__init__(route_class=route_class, **kwargs)
