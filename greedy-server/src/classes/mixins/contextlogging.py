import datetime as dt


class ContextLoggerMixin:
    def __enter__(self):
        return self

    def __exit__(self, exc_type, exc_val, exc_tb):
        if exc_type is not None:
            self._log_error(exc_type, exc_val)

    @staticmethod
    def _log_error(exc, val):
        data = {
            "exceptionType": exc.__name__, "exceptionValue": str(val), "exceptionThrowTime": dt.datetime.utcnow()
        }

        print(data)