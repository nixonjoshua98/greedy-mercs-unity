
from src import utils
from src.common import formulas


def get_artefacts() -> "ArtefactResources":
    return ArtefactResources(utils.load_resource("artefacts.json"))


class ArtefactResources:
    def __init__(self, data: dict):
        self.__dict = data

        self.artefacts: dict = {k: ArtefactData.from_dict(v) for k, v in data.items()}

    def as_dict(self): return self.__dict


class ArtefactData:
    __slots__ = ("cost_coeff", "cost_expo", "base_effect", "level_effect", "max_level")

    @classmethod
    def from_dict(cls, data: dict):
        inst = ArtefactData()

        inst.cost_expo = data["costExpo"]
        inst.cost_coeff = data["costCoeff"]
        inst.base_effect = data["baseEffect"]
        inst.level_effect = data["levelEffect"]

        inst.max_level = data.get("maxLevel", 1_000)

        return inst

    def upgrade_cost(self, level: int, buying: int):
        """ Calculate the upgrade cost

        Args:
            level (int): Current artefact level
            buying (int): Levels the user intended to upgrade

        Returns:
            int: Upgrade cost
        """
        return formulas.upgrade_artefact_cost(self.cost_coeff, self.cost_expo, level, buying)
