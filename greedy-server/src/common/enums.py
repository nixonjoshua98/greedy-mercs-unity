import enum


class EnumBase(enum.IntEnum):

    @classmethod
    def get_val(cls, val: int):
        val = int(val)

        if val in cls._value2member_map_:
            return cls(val)

        return None


class BonusType(EnumBase):
    PRESTIGE_BONUS = 500


class ItemKeys:
    BLUE_GEMS = "blueGems"
    ARMOURY_POINTS = "ironIngots"
    PRESTIGE_POINTS = "prestigePoints"
    BOUNTY_POINTS = "bountyPoints"


class ItemType(EnumBase):
    BLUE_GEMS = 100
    ARMOURY_POINTS = 200
    PRESTIGE_POINTS = 300
    BOUNTY_POINTS = 400

    @property
    def key(self):
        return getattr(ItemType, self.value)
