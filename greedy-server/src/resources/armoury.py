
from src import utils


def get_armoury_resources(*, as_dict: bool = False) -> "ArmouryResources":
    if as_dict:
        return utils.load_resource("armoury.json")

    return ArmouryResources(utils.load_resource("armoury.json"))


class ArmouryResources:
    def __init__(self, data: dict):
        self.max_evo_level = data["maxEvoLevel"]
        self.evo_level_cost = data["evoLevelCost"]

        self.items: dict[int, "ArmouryItem"] = {k: ArmouryItem.from_dict(v) for k, v in data["items"].items()}


class ArmouryItem:
    __slots__ = ("tier", "base_damage")

    @classmethod
    def from_dict(cls, data: dict):
        inst = ArmouryItem()

        inst.tier = data["itemTier"]
        inst.base_damage = data["baseDamageMultiplier"]

        return inst

    def level_cost(self, level: int) -> int:
        return 5 + (self.tier + 1) + level
