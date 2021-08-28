
from fastapi import FastAPI, HTTPException

from src.exceptions import handle_http_exception
from src.dataloader import DataLoader


def _on_app_start():
    DataLoader.create_client("mongodb://localhost:27017/g0")


def create_app():
    app = FastAPI(redoc_url=None, docs_url=None, openapi_url=None, swagger_ui_oauth2_redirect_url=None)

    app.add_exception_handler(HTTPException, handle_http_exception)
    app.add_event_handler("startup", _on_app_start)

    return app
