import functools as ft

from fastapi import HTTPException
from fastapi.exceptions import RequestValidationError

from src.application import Application
from src.mongo.motorclient import MotorClient

from .cache import MemoryCache
from src import exception_handlers


def _on_app_start(fast_app: Application):
    fast_app.state.mongo = MotorClient("mongodb://localhost:27017/g0")
    fast_app.state.memory_cache = MemoryCache()


def create_app():
    fast_app = Application(
        redoc_url=None,
        docs_url=None,
        openapi_url=None,
        swagger_ui_oauth2_redirect_url=None,
    )

    fast_app.add_exception_handler(HTTPException, exception_handlers.handle_http_exception)
    fast_app.add_exception_handler(RequestValidationError, exception_handlers.handle_request_validation_error)

    fast_app.add_event_handler("startup", ft.partial(_on_app_start, fast_app))

    return fast_app
