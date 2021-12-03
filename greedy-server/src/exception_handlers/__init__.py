from fastapi import HTTPException, Request
from fastapi.exceptions import RequestValidationError

from src import logger
from src.routing import ServerResponse
from src.routing.handlers.abc import BaseHandlerException


async def handle_http_exception(_: Request, exc: HTTPException):
    logger.warning(exc.detail)

    return ServerResponse(
        {"code": exc.status_code, "error": "Error"}, status_code=exc.status_code
    )


async def handle_request_validation_exception(_: Request, exc: RequestValidationError):
    logger.warning(exc.errors())

    return ServerResponse({"code": 400, "error": "Client request error"}, status_code=400)


async def handle_handler_exception(_: Request, exc: BaseHandlerException):
    d = {
        "code": exc.status_code if exc.status_code > 0 else 500,
        "error": exc.message if exc.message != "" else "Internal server error"
    }

    logger.warning(d)

    return ServerResponse(d, status_code=d["code"])
