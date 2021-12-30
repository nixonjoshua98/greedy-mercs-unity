import logging as _logging
import logging.config as _logging_config

from src import utils as _utils

_logging_config.dictConfig(_utils.yaml_load("logging.json"))

logger = _logging.getLogger("GM.Logger")
