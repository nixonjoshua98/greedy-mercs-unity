from src.loottable import LootTable

from ..armoury import StaticArmouryItem
from .shopconfig import LevelBountyShopConfig


def generate_from_config(
    s_armoury: list[StaticArmouryItem], config: LevelBountyShopConfig
):
    root = LootTable()

    _add_ai_tables(root, s_armoury, config)

    return root


def _add_ai_tables(
    root: LootTable, s_armoury: list[StaticArmouryItem], config: LevelBountyShopConfig
):
    """
    [Private] Add static armoury item loot tables to the root table
    :param root: Root loot table
    :param s_armoury: Static armoury data
    :param config: Bounty shop config
    """

    # Iterate over all armoury item tiers available for this shop
    for s_item in config.armoury_items:

        # Fetch all static armoury items for a single tier
        s_items = [it for it in s_armoury if it.tier == s_item.tier]

        # Rare occasion that no item exists in the tier provided (perhaps input error)
        if not s_items:
            continue

        t = LootTable()

        # Add all items to the table (with th same weight)
        for it in s_items:
            t.add_item(it)

        root.add_item(t, weight=s_item.weight)
