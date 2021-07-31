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
