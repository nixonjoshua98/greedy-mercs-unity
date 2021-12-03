class BaseHandlerException(Exception):
    status_code: int = 500
    message: str = ""

    def __init__(self, code: int = 500, message: str = ""):
        self.status_code = code
        self.message = message


class BaseHandler:
    ...


class BaseResponse:
    ...
