import enum


class BonusType(enum.IntEnum):
    ALL_MERC_DAMAGE     = 0,
    ENEMY_GOLD          = 1,
    TAP_DAMAGE          = 2,
    BOSS_GOLD           = 3,
    HERO_TAP_DAMAGE_ADD = 4,

    MELEE_DAMAGE        = 5,
    RANGED_DAMAGE       = 7,

    CRIT_CHANCE         = 8,
    ALL_GOLD            = 9,
    CRIT_DAMAGE         = 10,
    CASH_OUT_BONUS      = 11
