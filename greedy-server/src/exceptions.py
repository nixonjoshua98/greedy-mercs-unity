from fastapi import HTTPException


class AuthenticationError(HTTPException):
    ...

