from fastapi import Request, HTTPException
from fastapi.responses import JSONResponse


async def handle_http_exception(_: Request, exc: HTTPException):
    return JSONResponse(status_code=exc.status_code, content={"code": exc.status_code, "error": exc.detail})

