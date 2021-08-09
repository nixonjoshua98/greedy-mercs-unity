
import functools as ft

from fastapi import FastAPI, HTTPException

from src import db

from src.middleware import RequestStateMiddleware
from src.exceptions import handle_http_exception


def _on_app_start(app):
    app.state.mongo = db.create_client("mongodb://localhost:27017/g0")


def create_app():
    app = FastAPI(redoc_url=None, docs_url=None, openapi_url=None, swagger_ui_oauth2_redirect_url=None)

    app.add_middleware(RequestStateMiddleware)

    app.add_exception_handler(HTTPException, handle_http_exception)

    app.add_event_handler("startup", ft.partial(_on_app_start, app))

    return app
