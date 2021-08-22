
from fastapi import FastAPI, HTTPException

from src.exceptions import handle_http_exception


def create_app():
    app = FastAPI(redoc_url=None, docs_url=None, openapi_url=None, swagger_ui_oauth2_redirect_url=None)

    app.add_exception_handler(HTTPException, handle_http_exception)

    return app
