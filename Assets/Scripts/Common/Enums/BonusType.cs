namespace SRC.Common.Enums
{
    public enum BonusType
    {
        NONE = 0,

        // Critical Chance
        FLAT_CRIT_CHANCE = 300,

        // Critical Damage
        MULTIPLY_CRIT_DMG = 400,

        // Prestige
        MULTIPLY_PRESTIGE_BONUS = 500,

        // Damage
        MULTIPLY_ALL_DMG = 600,
        MULTIPLY_MERC_DMG = 601,
        MULTIPLY_MELEE_DMG = 602,
        MULTIPLY_RANGED_DMG = 603,

        // Tap Damage
        MULTIPLY_TAP_DMG = 700,

        // Gold
        MULTIPLY_ALL_GOLD = 800,
        MULTIPLY_BOSS_GOLD = 801,
        MULTIPLY_ENEMY_GOLD = 802,

        MULTIPLY_ARTEFACT_BONUS = 900
    }

    public static class BonusTypeExtensions
    {
        public static bool IsFlatValue(this BonusType bonusType)
        {
            return bonusType.ToString().StartsWith("FLAT_");
        }

        public static double DefaultValue(this BonusType bonusType)
        {
            return bonusType switch
            {
                BonusType.FLAT_CRIT_CHANCE => 0,
                _ => 1
            };
        }
    }
}