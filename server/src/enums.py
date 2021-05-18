import enum


class Enum(enum.IntEnum):

    @classmethod
    def get_val(cls, val: int):
        val = int(val)

        if val in cls._value2member_map_:
            return cls(val)

        return None


class BonusType(enum.IntEnum):
    CASH_OUT_BONUS = 11


class BountyShopItemID(Enum):

    PRESTIGE_POINTS = 100
    GEMS            = 101
    ARMOURY_POINTS  = 102

    ARMOURY_CHEST_A = 300
    ARMOURY_CHEST_B = 301
    ARMOURY_CHEST_C = 302
