import functools as ft

from fastapi import HTTPException
from fastapi.exceptions import RequestValidationError

from src import exception_handlers, utils
from src.application import Application
from src.auth import AuthenticationService
from src.handlers.abc import HandlerException
from src.mongo.motorclient import MotorClient
from src.redis import RedisClient


async def _on_app_start(app: Application):
    app.state.redis_client = redis_client = RedisClient(app.config.redis)

    app.state.mongo = MotorClient(app.config.mongo_con_str)
    app.state.auth_service = AuthenticationService(redis=redis_client)


def create_app():
    app = Application(
        redoc_url=None,
        docs_url=None,
        openapi_url=None,
        swagger_ui_oauth2_redirect_url=None,
    )

    app.add_exception_handler(HTTPException, exception_handlers.handle_http_exception)
    app.add_exception_handler(RequestValidationError, exception_handlers.handle_validation_exception)
    app.add_exception_handler(HandlerException, exception_handlers.handle_handler_exception)

    app.add_event_handler("startup", ft.partial(_on_app_start, app))

    return app
