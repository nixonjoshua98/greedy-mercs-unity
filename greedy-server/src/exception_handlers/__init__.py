from fastapi import HTTPException, Request
from fastapi.exceptions import RequestValidationError

from src.handlers.abc import HandlerException
from src.loggers import logger
from src.request import ServerRequest
from src.response import ServerResponse


async def handle_http_exception(_request: ServerRequest, exc: HTTPException):
    logger.error(exc.detail)

    return ServerResponse({"code": exc.status_code, "error": exc.detail}, status_code=exc.status_code)


async def handle_validation_exception(_request: Request, exc: RequestValidationError):
    logger.warn(exc.errors())

    return ServerResponse({"code": 400, "error": "Request error"}, status_code=400)


async def handle_handler_exception(_request: Request, exc: HandlerException):
    logger.debug(exc.message)

    return ServerResponse({"code": exc.status_code, "error": exc.message}, status_code=exc.status_code)
