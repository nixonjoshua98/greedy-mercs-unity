using UnityEngine;

namespace GM.Common
{
    public static class Constants
    {
        public const float CENTER_BATTLE_Y = 12f;

        public const float BASE_CRIT_CHANCE = 0.01f;
        public const float BASE_CRIT_MULTIPLIER = 3.0f;

        public static class Headers
        {
            public const string ResponseEncrypted = "Response-Encrypted";
            public const string InvalidToken = "Invalid-Token";
        }

        public static class Tags
        {
            public const string BossHealthBar = "EnemyBossUnitHealthBar";
            public const string MainCanvas = "MainCanvas";
        }

        public static class Colors
        {
            public static readonly Color Green = Color255(32, 200, 0);
            public static readonly Color Orange = Color255(216, 128, 8);
            public static readonly Color Red = Color255(255, 0, 0);
            public static readonly Color SoftBlue = Color255(0, 128, 255);
            public static readonly Color Grey = Color255(128, 128, 128);
            public static readonly Color Gold = Color255(212, 175, 55);
            public static readonly Color Yellow = Color255(255, 165, 0);

            private static Color Color255(float r, float g, float b)
            {
                return new(r / 255, g / 255, b / 255);
            }
        }
    }
}