using System;
using System.Numerics;

using UnityEngine;

namespace GreedyMercs
{
    using GM.Armoury;
    
    public static class Formulas
    {
        public static class StageEnemy
        {
            public static float SpawnDelay
            {
                get
                {
                    return 0.25f;
                }
            }

            #region Health
            public static BigDouble CalcEnemyHealth(int stage)
            {
                BigDouble x = BigDouble.Pow(1.35, Mathf.Min(stage - 1, 65));
                BigDouble y = BigDouble.Pow(1.16, BigDouble.Parse(Mathf.Max(stage - 65, 0).ToString()));

                return 15 * x * y;
            }

            public static BigDouble CalcBossHealth(int stage) => CalcEnemyHealth(stage) * (stage % 5 == 0 ? 5.0f : 3.0f);
            #endregion

            #region Gold
            public static BigDouble CalcEnemyGold(int stage) => 10.0f * CalcEnemyHealth(stage) * (0.0075 + (0.0002 * Mathf.Max(0, 100 - (stage - 1))));

            public static BigDouble CalcBossGold(int stage) => CalcEnemyGold(stage) * 5.0f;

            #endregion
        }

        public static class Armoury
        {
            public static double WeaponDamage(int index)
            {
                ArmouryItemState state = ArmouryManager.Instance.GetItem(index);

                return WeaponDamage(index, state.level, state.evoLevel);
            }

            public static double WeaponDamage(int index, int level)
            {
                ArmouryItemState state = ArmouryManager.Instance.GetItem(index);

                return WeaponDamage(index, level, state.evoLevel);
            }

            public static double WeaponDamage(int index, int level, int evoLevel)
            {
                ArmouryItemData itemData = StaticData.Armoury.Get(index);

                double val = (evoLevel + 1) * ((itemData.BaseDamageMultiplier) - 1) * level;

                val += 1;

                return val > 1 ? val : 0;
            }
        }

        // =====

        public static BigDouble CalcEnemyGold(int stage)
        {
            return 10.0f * StageEnemy.CalcEnemyHealth(stage) * (0.008 + (0.0002 * Mathf.Max(0, 100 - (stage - 1))));
        }

        public static BigDouble CalcBossGold(int stage)
        {
            return CalcEnemyGold(stage) * 5.0f;
        }

        // ===

        public static BigDouble CalcCharacterDamage(CharacterID chara)
        {
            var state           = GameState.Characters.Get(chara);
            CharacterSO data    = StaticData.CharacterList.Get(chara);

            BigDouble baseDamage = data.baseDamage > 0 ? data.baseDamage : (data.unlockCost / (10.0f + BigDouble.Log10(data.unlockCost)));

            return baseDamage * state.level * BigDouble.Pow(2.0f, (state.level - 1) / 100.0f) * (1 - (0.035f * data.unlockOrder));
        }

        // ===

        public static BigDouble CalcCharacterLevelUpCost(CharacterID chara, int levels)
        {
            var state           = GameState.Characters.Get(chara);
            CharacterSO data    = StaticData.CharacterList.Get(chara);

            return BigMath.SumGeometricSeries(levels, data.unlockCost, 1.075 + (data.unlockOrder / 1000.0), state.level);
        }

        public static int AffordCharacterLevels(CharacterID chara)
        {
            UpgradeState state  = GameState.Characters.Get(chara);
            CharacterSO data    = StaticData.CharacterList.Get(chara);

            BigDouble val = BigMath.AffordGeometricSeries(GameState.Player.gold, data.unlockCost, 1.075 + (data.unlockOrder / 1000.0), state.level);

            return Mathf.Min(StaticData.MAX_CHAR_LEVEL - state.level, int.Parse(val.ToString()));
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

            int maxLevels = int.Parse(BigMath.AffordGeometricSeries(GameState.Player.gold, 5, 1.09, state.level - 1).ToString());

            return Mathf.Min(StaticData.MAX_TAP_UPGRADE_LEVEL - state.level, maxLevels);
        }

        // === Prestige Points ===

        public static BigInteger CalcPrestigePoints(int stage)
        {
            if (stage < StageState.MIN_PRESTIGE_STAGE)
                return 0;

            BigDouble big = BigDouble.Pow(Mathf.CeilToInt((stage - 75) / 10.0f), 2.2);

            return BigInteger.Parse(big.Ceiling().ToString("F0"));
        }

        // ===

        public static BigInteger CalcNextLootCost(int numRelics)
        {
            return BigInteger.Parse((Mathf.Max(1, numRelics - 2) * BigDouble.Pow(1.35, numRelics).Floor()).ToString("F0"));
        }

        // ===

        public static double CalcLootItemEffect(LootID relic)
        {
            LootItemSO data = StaticData.LootList.Get(relic);
            UpgradeState state = GameState.Loot.Get(relic);

            return data.baseEffect + (data.levelEffect * (state.level - 1));
        }

        // === Relics ===

        public static BigInteger CalcLootItemLevelUpCost(LootID relic, int levels)
        {
            LootItemSO data = StaticData.LootList.Get(relic);
            UpgradeState state = GameState.Loot.Get(relic);

            return (data.costCoeff * SumNonIntegerPowerSeq(state.level, levels, data.costExpo)).ToBigInteger();
        }

        // === General ===

        public static BigDouble SumNonIntegerPowerSeq(int level, int levelsBuying, float s)
        {
            // https://math.stackexchange.com/questions/82588/is-there-a-formula-for-sums-of-consecutive-powers-where-the-powers-are-non-inte


            BigDouble Predicate(int startValue)
            {
                BigDouble x = Mathf.Pow(startValue, s + 1) / (s + 1);
                BigDouble y = Mathf.Pow(startValue, s) / 2;
                BigDouble z = Mathf.Sqrt(Mathf.Pow(startValue, s - 1));

                return x + y + z;
            }

            return Predicate(level + levelsBuying) - Predicate(level);
        }
    }
}