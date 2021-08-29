
from .armoury import get_armoury_resources, ArmouryResources
from .artefacts import get_artefacts_data, ArtefactResources
from .bounties import get_bounty_data, BountyResources


def get_bounty_shop(uid, *, as_dict: bool = False):
    from src.classes.bountyshop import BountyShopGeneration

    bs = BountyShopGeneration(uid)

    return bs.to_dict() if as_dict else bs
