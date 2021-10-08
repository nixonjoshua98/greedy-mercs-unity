
from .armoury import get_armoury_resources, ArmouryResources
from .artefacts import get_artefacts_data, ArtefactResources
from .bounties import get_bounty_data, BountyResources


def get_bounty_shop(uid):
    from src.resources.bountyshop import BountyShop

    return BountyShop().to_dict()
