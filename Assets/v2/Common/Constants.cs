using UnityEngine;

namespace GM.Common
{
    public static class Constants
    {
        public const int MAX_SQUAD_SIZE = 5;

        public const int MIN_PRESTIGE_STAGE = 60;

        public const float CENTER_BATTLE_Y = 12.5f;
        public const int MAX_MERC_LEVEL = 1_000;

        public const float BASE_CRIT_CHANCE = 0.01f;
        public const float BASE_CRIT_MULTIPLIER = 3.0f;

        public static class Tags
        {
            public static string EnemyBossUnitHealthBar = "EnemyBossUnitHealthBar";
        }

        public static class Colors
        {
            public static Color SoftRed = Color255(170, 0, 0);
            public static Color SoftBlue = Color255(0, 128, 255);
            public static Color Orange = Color255(255, 165, 0);

            static Color Color255(float r, float g, float b) => new Color(r / 255, g / 255, b / 255);
        }
    }
}