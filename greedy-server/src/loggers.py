import logging.config
import os

from src import utils as _utils

logging.config.dictConfig(_utils.yaml_load("logging.json"))

os.makedirs(os.path.join(os.getcwd(), "logs"), exist_ok=True)

logger = logging.getLogger("GM.Logger")
