from typing import Optional

from fastapi import HTTPException


class ServerException(HTTPException):
    def __init__(self, status_code, detail: Optional[str] = None):
        super(ServerException, self).__init__(status_code=status_code, detail=detail or "")


class HandlerException(ServerException):
    ...
