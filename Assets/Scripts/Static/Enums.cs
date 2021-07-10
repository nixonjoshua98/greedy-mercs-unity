
public enum BonusType
{
    NONE = -1,

    // Energy Income (Per Minute)
    FLAT_ENERGY_CAPACITY = 100,
    PERCENT_ENERGY_CAPACITY = 101,

    // Energy Capacity
    FLAT_ENERGY_INCOME = 200,
    PERCENT_ENERGY_INCOME = 201,

    // Critical Hits
    FLAT_CRIT_CHANCE = 300,
    FLAT_CRIT_DMG_MULT = 301,

    MERC_DAMAGE = 0,
    ENEMY_GOLD = 1,
    TAP_DAMAGE = 2,
    BOSS_GOLD = 3,
    CHAR_TAP_DAMAGE_ADD = 4,
    MELEE_DAMAGE = 5,
    RANGED_DAMAGE = 7,
    ALL_GOLD = 9,
    CASH_OUT_BONUS = 11,
    GOLD_RUSH_BONUS = 14,
    GOLD_RUSH_DURATION = 15,
    AUTO_CLICK_BONUS = 16,
    AUTO_CLICK_DURATION = 17,
    BOSS_TIMER_DURATION = 18,
}

public enum UnitID { ZERO, ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE }

public enum GoldUpgradeID
{
    TAP_DAMAGE = 0
}