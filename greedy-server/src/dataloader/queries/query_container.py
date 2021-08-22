import abc


class DatabaseQueryContainer(abc.ABC):
    def __init__(self, client):
        self.client = client

    @property
    def default_database(self):
        return self.client.get_default_database()

