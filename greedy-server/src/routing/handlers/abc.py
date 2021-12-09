class BaseHandlerException(Exception):
    def __init__(self, code: int = 500, message: str = ""):
        self.status_code: int = code
        self.message: str = message


class BaseHandler:
    ...


class BaseResponse:
    ...
