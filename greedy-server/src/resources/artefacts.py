
from src import utils
from src.common import formulas


def get_artefacts_data(*, as_dict: bool = False, as_list=False) -> "ArtefactResources":
    if as_list:
        return [{"artefactId": k, **v} for k, v in utils.load_resource("artefacts.json").items()]  # type: ignore

    if as_dict:
        return utils.load_resource("artefacts.json")

    return ArtefactResources(utils.load_resource("artefacts.json"))


class ArtefactResources:
    def __init__(self, data: dict):
        self.artefacts: dict = {k: ArtefactResourceData.from_dict(v) for k, v in data.items()}


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
