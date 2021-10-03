import os
import logging as _logging
from logging.handlers import RotatingFileHandler

LOG_FILE_PATH = os.path.join(os.getcwd(), "logs", "log.txt")
MESSAGE_FORMAT = "%(asctime)s [%(levelname)s] %(message)s"
DATE_FORMAT = "%d/%m/%Y %H:%M:%S"

logger: _logging.Logger = _logging.getLogger(__name__)


def info(*args, **kwargs): logger.info(*args, **kwargs)
def warning(*args, **kwargs): logger.warning(*args, **kwargs)
def error(*args, **kwargs): logger.error(*args, **kwargs)


def _create_console_handler(level: int):
    handler = _logging.StreamHandler()

    handler.setLevel(level)

    handler.setFormatter(_logging.Formatter(MESSAGE_FORMAT, datefmt=DATE_FORMAT))

    return handler


def _create_file_handler(level: int, filepath: str):
    os.makedirs(os.path.dirname(filepath), exist_ok=True)

    handler = RotatingFileHandler(filename=filepath, backupCount=5, maxBytes=1_000_000)

    handler.setLevel(level)

    handler.setFormatter(_logging.Formatter(MESSAGE_FORMAT, datefmt=DATE_FORMAT))

    return handler


logger.setLevel(_logging.INFO)

logger.addHandler(_create_console_handler(_logging.INFO))
logger.addHandler(_create_file_handler(_logging.WARNING, LOG_FILE_PATH))
