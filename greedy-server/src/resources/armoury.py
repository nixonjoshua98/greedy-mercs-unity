
from src import utils


def get_armoury() -> "ArmouryResource":
    return ArmouryResource(utils.load_resource("armoury.json"))


class ArmouryResource:
    def __init__(self, data: dict):
        self.__dict = data

        self.max_evo_level = data["maxEvoLevel"]
        self.evo_level_cost = data["evoLevelCost"]

        self.items: dict["ArmouryItem"] = {k: ArmouryItem.from_dict(v) for k, v in data["items"].items()}

    def as_dict(self):
        return self.__dict


class ArmouryItem:
    __slots__ = ("tier", "base_damage")

    @classmethod
    def from_dict(cls, data: dict):
        inst = ArmouryItem()

        inst.tier = data["itemTier"]
        inst.base_damage = data["baseDamageMultiplier"]

        return inst

    def level_cost(self, level: int) -> int:
        return 5 + self.tier + level
