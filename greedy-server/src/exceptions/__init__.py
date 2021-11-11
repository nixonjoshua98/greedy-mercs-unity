from fastapi import HTTPException, Request

from src import logger
from src.routing import ServerResponse


async def handle_http_exception(req: Request, exc: HTTPException):
    logger.debug(exc.detail)

    return ServerResponse(
        {"code": exc.status_code, "error": exc.detail}, status_code=exc.status_code
    )
