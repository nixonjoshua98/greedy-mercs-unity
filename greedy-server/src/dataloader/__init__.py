
from .mongo.mongocontroller import MongoController
from .serverstate import ServerState

"""
    ================================================================
    DataLoader is mainly used as a container which stores multiple data sources such as the database, data files, and
    server state. Data pulled from this class should only be relevant to the game, NOT config files etc.
    ================================================================
"""


class DataLoader:
    def __init__(self):
        self.mongo = MongoController()

        self._server_state = None

    def get_server_state(self) -> ServerState:

        # Cache the result so the state remains the same for repeated calls
        if self._server_state is None:
            self._server_state = ServerState()

        return self._server_state

    # Context Manager Methods #

    def __enter__(self):
        return self

    def __exit__(self, exc_type, exc_val, exc_tb):
        return None
