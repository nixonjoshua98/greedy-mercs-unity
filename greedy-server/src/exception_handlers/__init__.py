from fastapi import HTTPException, Request
from fastapi.exceptions import RequestValidationError

from src import logger
from src.routing import ServerResponse, ServerRequest
from src.routing.handlers.abc import HandlerException


async def handle_http_exception(request: ServerRequest, exc: HTTPException):
    logger.warning(exc.detail)

    error = exc.detail if request.app.debug else "Server error"

    return ServerResponse({"code": exc.status_code, "error": error}, status_code=exc.status_code)


async def handle_request_validation_exception(_: Request, __: RequestValidationError):
    return ServerResponse({"code": 400, "error": "Client request error"}, status_code=400)


async def handle_handler_exception(_: Request, exc: HandlerException):
    logger.debug(f"{exc.status_code} - {exc.message}")

    return ServerResponse({"code": exc.status_code, "error": exc.message}, status_code=exc.status_code)
