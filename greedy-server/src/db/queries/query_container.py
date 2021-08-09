import abc


class DatabaseQueryContainer(abc.ABC):
    def __init__(self, client):
        self.client = client
