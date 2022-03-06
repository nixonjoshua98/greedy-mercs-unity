from src import utils


class StaticFilesService:

    @staticmethod
    def load_mercs() -> dict: return utils.load_static_data_file("mercs.json5")

    @staticmethod
    def load_artefacts() -> list: return utils.load_static_data_file("artefacts.json")
