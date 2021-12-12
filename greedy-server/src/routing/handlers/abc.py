class HandlerException(Exception):
    def __init__(self, code: int = 500, message: str = "Internal server error"):
        self.status_code: int = code
        self.message: str = message


class BaseHandler:
    ...


class BaseResponse:
    ...
