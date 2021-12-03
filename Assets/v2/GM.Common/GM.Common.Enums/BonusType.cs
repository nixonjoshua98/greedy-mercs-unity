namespace GM.Common.Enums
{
    public enum BonusType
    {
        // Critical Chance
        FLAT_CRIT_CHANCE = 300,

        // Critical Damage
        FLAT_CRIT_DMG = 400,

        // Prestige
        MULTIPLY_PRESTIGE_BONUS = 500,

        // Damage
        MULTIPLY_ALL_DMG = 600,
        MULTIPLY_MERC_DMG = 601,
        MULTIPLY_MELEE_DMG = 602,
        MULTIPLY_RANGED_DMG = 603,

        // Tap Damage
        FLAT_TAP_DAMAGE = 700,

        // Gold
        MULTIPLY_ALL_GOLD = 800,
        MULTIPLY_BOSS_GOLD = 801,
        MULTIPLY_ENEMY_GOLD = 802,

        MERC_DAMAGE = 0,
        ENEMY_GOLD = 1,
        BOSS_GOLD = 3,
        MELEE_DAMAGE = 5,
        RANGED_DAMAGE = 7,
        ALL_GOLD = 9,
    }
}