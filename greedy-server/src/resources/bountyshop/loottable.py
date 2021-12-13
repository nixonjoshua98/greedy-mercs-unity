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

    t = LootTable()

    for it in s_armoury:
        t.add_item(it)

    root.add_item(t, weight=config.armoury_items.weight)
