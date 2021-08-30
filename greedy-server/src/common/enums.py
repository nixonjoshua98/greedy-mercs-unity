import enum


class BonusType:
    PRESTIGE_BONUS = 500


class ItemKey:
    """ ItemType and ItemKey should share the same attribute names """

    BLUE_GEMS = "blueGems"
    ARMOURY_POINTS = "armouryPoints"
    PRESTIGE_POINTS = "prestigePoints"
    BOUNTY_POINTS = "bountyPoints"


class ItemType(enum.IntEnum):
    BLUE_GEMS = 100
    ARMOURY_POINTS = 200
    PRESTIGE_POINTS = 300
    BOUNTY_POINTS = 400

    @property
    def key(self) -> ItemKey:
        return getattr(ItemKey, self.name)
