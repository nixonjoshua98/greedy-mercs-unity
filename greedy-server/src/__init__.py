import functools as ft

from fastapi import FastAPI, HTTPException

from src.exceptions import handle_http_exception
from src.dataloader import DataLoader
from src.cache import MemoryCache

from motor.motor_asyncio import AsyncIOMotorClient


def _on_app_start(app):
    DataLoader.create_client("mongodb://localhost:27017/g0")

    app.state.mongo = AsyncIOMotorClient("mongodb://localhost:27017/g0")
    app.state.memory_cache = MemoryCache()


def create_app():
    app = FastAPI(redoc_url=None, docs_url=None, openapi_url=None, swagger_ui_oauth2_redirect_url=None)

    app.add_exception_handler(HTTPException, handle_http_exception)
    app.add_event_handler("startup", ft.partial(_on_app_start, app))

    return app