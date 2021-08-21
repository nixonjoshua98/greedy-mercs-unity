using System;
using System.Numerics;

using UnityEngine;

namespace GM
{
    using GM.Data;
    using GM.Armoury;
    using GM.Units;
    
    public static class Formulas
    {
        // = = = Artefacts = = = /
        public static BigInteger ArtefactLevelUpCost(int currentLevel, int levels, float expo, float coeff)
        {
            return (coeff * SumNonIntegerPowerSeq(currentLevel, levels, expo)).ToBigInteger();
        }

        public static double BaseArtefactEffect(int currentLevel, double baseEffect, double levelEffect)
        {
            return baseEffect + (levelEffect * (currentLevel - 1));
        }

        // = = = Mercs = = = //
        public static BigDouble MercLevelUpCost(int currentLevel, int levelsBuying, double unlockCost)
        {
            return BigMath.SumGeometricSeries(levelsBuying, unlockCost, 1.077, currentLevel);
        }

        public static BigDouble MercBaseDamage(BigDouble baseDamage, int level)
        {
            return baseDamage * level * BigDouble.Pow(1.99f, (level - 1) / 100.0f) * (1 - 0.035f);
        }

        // = = = Enemies = = = //
        public static BigDouble EnemyHealth(int stage)
        {
            BigDouble x = BigDouble.Pow(1.35, Mathf.Min(stage - 1, 65));
            BigDouble y = BigDouble.Pow(1.16, BigDouble.Parse(Mathf.Max(stage - 65, 0).ToString()));

            return 15.0 * x * y;
        }

        public static BigDouble BossHealth(int stage)
        {
            return EnemyHealth(stage) * 5.5f;
        }

        // =====

        public static BigDouble CalcEnemyGold(int stage)
        {
            return 10.0f * EnemyHealth(stage) * (0.01 + (0.0005 * Mathf.Max(0, 100 - (stage - 1))));
        }

        public static BigDouble CalcBossGold(int stage)
        {
            return CalcEnemyGold(stage) * 7.3f;
        }

        public static int AffordCharacterLevels(MercID merc)
        {
            MercState state = MercenaryManager.Instance.GetState(merc);

            MercData data = GameData.Get.Mercs.Get(merc);

            BigDouble val = BigMath.AffordGeometricSeries(UserData.Get.Inventory.Gold, data.UnlockCost, 1.075 + ((int)merc / 1000.0), state.Level);

            return Mathf.Min(StaticData.MAX_CHAR_LEVEL - state.Level, int.Parse(val.ToString()));
        }


        // # === Tap Damage === #
        public static BigDouble CalcTapDamage()
        {
            UpgradeState state = GameState.Upgrades.GetUpgrade(GoldUpgradeID.TAP_DAMAGE);

            return state.level * BigDouble.Pow(2.0f, (state.level - 1) / 50.0f);
        }

        public static BigDouble CalcTapDamageLevelUpCost(int levels)
        {
            UpgradeState state = GameState.Upgrades.GetUpgrade(GoldUpgradeID.TAP_DAMAGE);

            return BigMath.SumGeometricSeries(levels, 5, 1.09, (state.level - 1));
        }

        public static int AffordTapDamageLevels()
        {
            UpgradeState state = GameState.Upgrades.GetUpgrade(GoldUpgradeID.TAP_DAMAGE);

            int maxLevels = int.Parse(BigMath.AffordGeometricSeries(UserData.Get.Inventory.Gold, 5, 1.09, state.level - 1).ToString());

            return Mathf.Min(StaticData.MAX_TAP_UPGRADE_LEVEL - state.level, maxLevels);
        }

        // === Prestige Points ===

        public static BigInteger CalcPrestigePoints(int stage)
        {
            BigDouble big = BigDouble.Pow(Mathf.CeilToInt((stage - 75) / 10.0f), 2.2);

            return BigInteger.Parse(big.Ceiling().ToString("F0"));
        }

        // ===

        public static BigInteger CalcNextLootCost(int numRelics)
        {
            return BigInteger.Parse((Mathf.Max(1, numRelics - 2) * BigDouble.Pow(1.35, numRelics).Floor()).ToString("F0"));
        }

        // === General ===

        public static BigDouble SumNonIntegerPowerSeq(int level, int levelsBuying, float exponent)
        {
            // https://math.stackexchange.com/questions/82588/is-there-a-formula-for-sums-of-consecutive-powers-where-the-powers-are-non-inte


            BigDouble Predicate(int startValue)
            {
                BigDouble x = Mathf.Pow(startValue, exponent + 1) / (exponent + 1);
                BigDouble y = Mathf.Pow(startValue, exponent) / 2;
                BigDouble z = Mathf.Sqrt(Mathf.Pow(startValue, exponent - 1));

                return x + y + z;
            }

            return Predicate(level + levelsBuying) - Predicate(level);
        }
    }
}