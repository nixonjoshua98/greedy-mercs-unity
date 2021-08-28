from typing import Union

from src import utils


def get_bounty_data(*, as_dict: bool = False) -> "BountyResources":
    if as_dict:
        return utils.load_resource("bounties.json")

    return BountyResources(utils.load_resource("bounties.json"))


class BountyResources:
    def __init__(self, data: dict):
        self.__dict = dict

        self.max_unclaimed_hours: Union[int, float] = data["maxUnclaimedHours"]
        self.max_active_bounties: int = data["maxActiveBounties"]

        self.bounties: dict[int, BountyData] = {k: BountyData(v) for k, v in data["bounties"].items()}

    def to_dict(self):
        return self.__dict


class BountyData:
    def __init__(self, data: dict):
        self.__dict = data

        self.income: Union[float, int] = data["hourlyIncome"]
        self.stage: int = data["unlockStage"]

    def as_dict(self) -> dict:
        return self.__dict
