from typing import Union

from src import utils


def get_bounty_data(*, as_dict: bool = False, as_list=False) -> "BountyResources":
    if as_dict:
        return utils.load_resource("bounties.json")

    elif as_list:
        d = utils.load_resource("bounties.json")

        bounties = d["bounties"]
        d["bounties"] = []

        for k, v in bounties.items():
            d["bounties"].append({"bountyId": k, **v})

        return d

    return BountyResources(utils.load_resource("bounties.json"))


class BountyResources:
    def __init__(self, data: dict):
        self.max_unclaimed_hours: Union[int, float] = data["maxUnclaimedHours"]
        self.max_active_bounties: int = data["maxActiveBounties"]

        self.bounties: dict[int, BountyData] = {k: BountyData(v) for k, v in data["bounties"].items()}


class BountyData:
    def __init__(self, data: dict):
        self.income: Union[float, int] = data["hourlyIncome"]
        self.stage: int = data["unlockStage"]
