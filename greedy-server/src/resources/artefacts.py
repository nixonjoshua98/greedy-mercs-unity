
from src import utils
from src.common import formulas


def get_artefacts_data(*, as_dict: bool = False) -> "ArtefactResources":
    if as_dict:
        return utils.load_resource("artefacts.json")

    return ArtefactResources(utils.load_resource("artefacts.json"))


class ArtefactResources:
    def __init__(self, data: dict):
        self.__dict = data

        self.artefacts: dict = {k: ArtefactResourceData.from_dict(v) for k, v in data.items()}

    def as_dict(self): return self.__dict


class ArtefactResourceData:
    __slots__ = ("cost_coeff", "cost_expo", "base_effect", "level_effect", "max_level")

    @classmethod
    def from_dict(cls, data: dict):
        inst = ArtefactResourceData()

        inst.cost_expo = data["costExpo"]
        inst.cost_coeff = data["costCoeff"]
        inst.base_effect = data["baseEffect"]
        inst.level_effect = data["levelEffect"]

        inst.max_level = data.get("maxLevel", 1_000)

        return inst

    def upgrade_cost(self, level: int, buying: int):
        return formulas.upgrade_artefact_cost(self.cost_coeff, self.cost_expo, level, buying)
