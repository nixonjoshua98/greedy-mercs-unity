
public enum BonusType
{
    NONE = 0,

    // Energy Capacity
    FLAT_ENERGY_CAPACITY = 100,

    // Energy Income (Per Minute)
    FLAT_ENERGY_INCOME = 200,

    // Critical Chance
    FLAT_CRIT_CHANCE = 300,

    // Critical Damage
    FLAT_CRIT_DMG = 400,

    // Prestige
    PERCENT_PRESTIGE_BONUS = 500,

    MERC_DAMAGE = 0,
    ENEMY_GOLD = 1,
    TAP_DAMAGE = 2,
    BOSS_GOLD = 3,
    CHAR_TAP_DAMAGE_ADD = 4,
    MELEE_DAMAGE = 5,
    RANGED_DAMAGE = 7,
    ALL_GOLD = 9,
    GOLD_RUSH_BONUS = 14,
    GOLD_RUSH_DURATION = 15,
    AUTO_CLICK_BONUS = 16,
    AUTO_CLICK_DURATION = 17
}

public enum MercID
{ 
    NONE = -1,

    STONE_GOLEM, 
    REAPER_MAN, 
    WRAITH, 
    FALLEN, 
    SKELETON_ARCHER, 
    MINOTAUR, 
    FIRE_GOLEM, 
    NECROMANCER, 
    DEMON_KNIGHT, 
    FIRE_SATYR 
}


public enum GoldUpgradeID
{
    TAP_DAMAGE = 0
}