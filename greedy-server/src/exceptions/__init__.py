from fastapi import Request, HTTPException
from fastapi.responses import JSONResponse

from src import logger


async def handle_http_exception(req: Request, exc: HTTPException):
    logger.error(f"{req.url} - {exc.status_code} - {exc.detail}")

    return JSONResponse(status_code=exc.status_code, content={"code": exc.status_code, "error": exc.detail})

