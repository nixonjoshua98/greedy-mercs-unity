from enum import IntEnum
from typing import NewType, Union

Number = Union[float, int]
QuestID = NewType("QuestID", int)
MercID = NewType("MercID", int)


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


class QuestType(IntEnum):
    MERC_QUEST = 0
