import enum


class BonusType(enum.IntEnum):
    MERC_DAMAGE = 0,
    ENEMY_GOLD = 1,
    TAP_DAMAGE = 2,
    BOSS_GOLD = 3,
    CHAR_TAP_DAMAGE_ADD = 4,

    MELEE_DAMAGE = 5,
    RANGED_DAMAGE = 7,

    CRIT_CHANCE = 8,
    ALL_GOLD = 9,
    CRIT_DAMAGE = 10,
    CASH_OUT_BONUS = 11,
    ENERGY_INCOME = 12,
    ENERGY_CAPACITY = 13,

    GOLD_RUSH_BONUS = 14,
    GOLD_RUSH_DURATION = 15,

    AUTO_TAPS = 16,
    BOSS_TIMER_DUR = 17


class ValueType(enum.IntEnum):
    MULTIPLY            = 0,
    ADDITIVE_PERCENT    = 1,
    ADDITIVE_FLAT_VAL   = 2


class BountyShopItem(enum.IntEnum):

    PRESTIGE_POINTS_PERCENT = 0,
    SMALL_GEM_PACK = 1
