import os
import logging.config

from src import utils as _utils

os.makedirs(os.path.join(os.getcwd(), "logs"), exist_ok=True)

logging.config.dictConfig(_utils.yaml_load("logging.json"))

logger = logging.getLogger("GM.Logger")
