using System;

using UnityEngine;

namespace SRC.Common
{
    /*
        Values calculated in this class should not require access to the game state and should be deterministic - which makes them easy to cache for a small performance increase
     */

    public static class GameFormulas
    {
        private static readonly TTLCache _cache = new();

        #region Minor Tap Upgrade
        public static BigDouble TapUpgradeDamage(int currentLevel)
        {
            return currentLevel * BigDouble.Pow(2.0f, (currentLevel - 1) / 25.0f);
        }

        public static BigDouble TapUpgradeUpgradeCost(int currentLevel, int levels)
        {
            return BigMath.SumGeometricSeries(levels, 5, 1.09f, (currentLevel - 1));
        }
        #endregion

        #region Major Tap Upgrade
        public static BigDouble MajorTapUpgradeBonusValue(int currentLevel)
        {
            return (currentLevel * 1.25f) * BigDouble.Pow(1.55f, (currentLevel - 1) / 35.0f);
        }

        public static BigDouble MajorTapUpgradeCost(int currentLevel, int levels)
        {
            return BigMath.SumGeometricSeries(levels, 1_000_000_000, 1.1f, currentLevel - 0);
        }
        #endregion

        #region Artefacts
        public static double ArtefactUnlockCost(int owned)
        {
            return Mathf.Max(1, owned - 2) * Math.Pow(1.35, owned);
        }

        public static BigDouble ArtefactBonusValue(int currentLevel, double baseEffect, double levelEffect)
        {
            return baseEffect + (levelEffect * (currentLevel - 1));
        }

        public static double ArtefactUpgradeCost(int currentLevel, int levels, float expo, float coeff)
        {
            return (coeff * SumNonIntegerPowerSeq(currentLevel, levels, expo)).ToDouble();
        }
        #endregion

        #region Armoury
        public static double ArmouryItemBonusValue(int level, int owned, float baseEffect, float levelEffect)
        {
            return (baseEffect + (levelEffect * ((level + owned) - 1)));
        }

        public static int ArmouryItemUpgradeCost(int level)
        {
            return 5 + level;
        }
        #endregion

        #region Mercs
        public static BigDouble MercUpgradeCost(int currentLevel, int levelsBuying)
        {
            return BigMath.SumGeometricSeries(levelsBuying, 25, 1.09f, (currentLevel - 1));
        }

        public static BigDouble MercBaseDamageAtLevel(BigDouble baseDamage, int level)
        {
            return baseDamage * level * BigDouble.Pow(baseDamage, (level - 1) / 50.0f);
        }
        #endregion

        #region Enemy Health
        public static BigDouble EnemyHealth(int stage)
        {
            BigDouble x = BigDouble.Pow(1.31, Mathf.Min(stage - 1, 65));
            BigDouble y = BigDouble.Pow(1.16, BigDouble.Max(stage - 65, 0));

            return 15.0 * x * y;
        }

        public static BigDouble BossHealth(int stage)
        {
            return EnemyHealth(stage) * 5;
        }
        #endregion

        // =====

        public static BigDouble CalcEnemyGold(int stage)
        {
            return 10.0f * EnemyHealth(stage) * (0.005 + (0.0004 * Mathf.Max(0, 25 - (stage - 1))));
        }

        public static BigDouble CalcBossGold(int stage)
        {
            return CalcEnemyGold(stage) * 7.3f;
        }

        /// <summary>
        /// Calculate the base prestige points for a provided stage
        /// </summary>
        public static double BasePrestigePoints(int stage)
        {
            string key = $"{nameof(BasePrestigePoints)}:{stage}";

            return _cache.Get<double>(key, () =>
            {
                double value = Math.Pow(Mathf.CeilToInt((stage - 75) / 10.0f), 2.2);

                if (double.IsNaN(value) || double.IsNegative(value))
                    return (double)0;

                return value;
            });
        }

        public static BigDouble SumNonIntegerPowerSeq(int start, int total, float exponent)
        {
            // https://math.stackexchange.com/questions/82588/is-there-a-formula-for-sums-of-consecutive-powers-where-the-powers-are-non-inte

            BigDouble Predicate(int startValue)
            {
                BigDouble x = Mathf.Pow(startValue, exponent + 1) / (exponent + 1);
                BigDouble y = Mathf.Pow(startValue, exponent) / 2;
                BigDouble z = Mathf.Sqrt(Mathf.Pow(startValue, exponent - 1));

                return x + y + z;
            }

            return Predicate(start + total) - Predicate(start);
        }
    }
}