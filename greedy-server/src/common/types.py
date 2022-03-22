from enum import IntEnum
from typing import NewType, Union

NumberType = Union[float, int]
QuestID = NewType("QuestID", int)
MercID = NewType("MercID", int)
ArtefactID = NewType("ArtefactID", int)
BountyID = NewType("BountyID", int)


class StatusCodes:
    INVALIDATE_CLIENT = 419


class QuestActionType(IntEnum):
    PRESTIGE = 0
    ENEMIES_DEFEATED = 1
    BOSSES_DEFEATED = 2
    TAPS = 3


class BonusType:
    NONE = 0

    FLAT_CRIT_CHANCE = 300

    MULTIPLY_CRIT_DMG = 400

    MULTIPLY_PRESTIGE_BONUS = 500

    MULTIPLY_ALL_DMG = 600
    MULTIPLY_MERC_DMG = 601
    MULTIPLY_MELEE_DMG = 602
    MULTIPLY_RANGED_DMG = 603

    MULTIPLY_TAP_DMG = 700
    FLAT_TAP_DMG = 701

    MULTIPLY_ALL_GOLD = 800
    MULTIPLY_BOSS_GOLD = 801
    MULTIPLY_ENEMY_GOLD = 802


class CurrencyType:
    ARMOURY_POINTS = 200


class AttackType:
    MELEE = 0
    RANGED = 1
