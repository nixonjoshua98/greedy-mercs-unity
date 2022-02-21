from fastapi import HTTPException, Request
from fastapi.exceptions import RequestValidationError

from src.loggers import logger
from src.routing import ServerRequest, ServerResponse
from src.routing.handlers.abc import HandlerException


async def handle_http_exception(_request: ServerRequest, exc: HTTPException):
    logger.error(exc.detail)

    return ServerResponse(
        {"code": exc.status_code, "error": exc.detail}, status_code=exc.status_code,
    )


async def handle_validation_exception(_request: Request, exc: RequestValidationError):
    logger.warn(f"Validation error - {exc.raw_errors}")

    return ServerResponse(
        {"code": 400, "error": "Client request error"}, status_code=400
    )


async def handle_handler_exception(_request: Request, exc: HandlerException):
    logger.debug(exc.message)

    return ServerResponse(
        {"code": exc.status_code, "error": exc.message}, status_code=exc.status_code
    )
