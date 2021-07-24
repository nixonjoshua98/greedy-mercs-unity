
from src import utils


def get_artefacts() -> "ArtefactResources":
    return ArtefactResources(utils.load_resource("artefacts.json"))


class ArtefactResources:
    def __init__(self, data: dict):
        self.__dict = data

        self.artefact: dict["Artefact"] = {k: Artefact.from_dict(v) for k, v in data.items()}

    def as_dict(self): return self.__dict


class Artefact:
    __slots__ = ("cost_coeff", "cost_expo", "base_effect", "level_effect")

    @classmethod
    def from_dict(cls, data: dict):
        inst = Artefact()

        inst.cost_expo = data["costExpo"]
        inst.cost_coeff = data["costCoeff"]
        inst.base_effect = data["baseEffect"]
        inst.level_effect = data["levelEffect"]

        return inst
