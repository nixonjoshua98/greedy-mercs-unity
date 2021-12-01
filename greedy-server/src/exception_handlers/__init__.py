from fastapi import HTTPException, Request

from fastapi.exceptions import RequestValidationError

from src import logger
from src.routing import ServerResponse


async def handle_http_exception(_: Request, exc: HTTPException):
    logger.debug(exc.detail)

    return ServerResponse(
        {"code": exc.status_code, "error": "Error"}, status_code=exc.status_code
    )


async def handle_request_validation_error(_: Request, exc: RequestValidationError):
    logger.debug(exc.errors())

    return ServerResponse({"code": 400, "error": "Client request error"}, status_code=400)
