from src import utils


class StaticDataService:

    @staticmethod
    def load_mercs() -> dict:
        return utils.load_static_data_file("mercs.json5")
