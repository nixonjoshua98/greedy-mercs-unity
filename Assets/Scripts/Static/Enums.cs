
public enum BonusType
{
    // Critical Chance
    FLAT_CRIT_CHANCE = 300,

    // Critical Damage
    FLAT_CRIT_DMG = 400,

    // Prestige
    MULTIPLY_PRESTIGE_BONUS = 500,

    // All Damage + Merc Damage
    MULTIPLY_ALL_DMG = 600,
    MULTIPLY_MERC_DMG = 601,
    MULTIPLY_MELEE_DMG = 602,
    MULTIPLY_RANGED_DMG = 603,

    // Tap Damage
    MULTIPLY_TAP_DMG = 700,
    MULTIPLY_TAP_DMG_FROM_MERC = 701,

    // Gold
    MULTIPLY_ALL_GOLD = 800,
    MULTIPLY_BOSS_GOLD = 801,
    MULTIPLY_ENEMY_GOLD = 802,

    MERC_DAMAGE = 0,
    ENEMY_GOLD = 1,
    TAP_DAMAGE = 2,
    BOSS_GOLD = 3,
    CHAR_TAP_DAMAGE_ADD = 4,
    MELEE_DAMAGE = 5,
    RANGED_DAMAGE = 7,
    ALL_GOLD = 9,
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
    JUNIOR_DARK_MAGE, 
    DEMON_KNIGHT, 
    FIRE_SATYR 
}


public enum GoldUpgradeID
{
    TAP_DAMAGE = 0
}