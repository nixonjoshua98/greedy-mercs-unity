using System.Collections.Generic;
using System.Numerics;
using Random = UnityEngine.Random;

namespace GM
{
    public class StatsCache : Core.GMClass
    {
        const float BASE_ENERGY_CAP = 50.0f;
        const float BASE_ENERGY_MIN = 1.0f;

        const float BASE_CRIT_CHANCE = 0.01f;
        const float BASE_CRIT_MULTIPLIER = 2.5f;
        
        static List<KeyValuePair<BonusType, double>> ArmouryBonus => App.Data.Armoury.Bonuses();
        static List<KeyValuePair<BonusType, double>> ArtefactBonus => App.Data.Arts.Bonuses();
        static List<KeyValuePair<BonusType, double>> CharacterBonus => App.Data.Mercs.Bonuses();

        public static BigDouble ArmouryMercDamageMultiplier { get { return AddSource(BonusType.MERC_DAMAGE, ArmouryBonus); } }

        public static class StageEnemy
        {
            public static BigDouble GetEnemyGold(int stage)
            {
                return (
                    Formulas.CalcEnemyGold(stage) *

                    MultiplyAllSources(BonusType.ENEMY_GOLD) * 
                    MultiplyAllSources(BonusType.ALL_GOLD)
                    );
            }

            public static BigDouble GetBossGold(int stage)
            {
                return (
                    Formulas.CalcBossGold(stage) *

                    MultiplyAllSources(BonusType.BOSS_GOLD) *
                    MultiplyAllSources(BonusType.ALL_GOLD)
                    );
            }
        }


        // # === Energy === #
        public static float EnergyPerMinute()
        {
            double flatExtraCapacity = AddAllSources(BonusType.FLAT_ENERGY_INCOME);

            return (float)(BASE_ENERGY_MIN + flatExtraCapacity);
        }

        public static float MaxEnergyCapacity()
        {
            double flatExtraCapacity = AddAllSources(BonusType.FLAT_ENERGY_CAPACITY);

            return (float)(BASE_ENERGY_CAP + flatExtraCapacity);
        }

        // = = = Critical Hits = = = //
        public static BigDouble CriticalHitChance()
        {
            return BASE_CRIT_CHANCE + 
                AddAllSources(BonusType.FLAT_CRIT_CHANCE);
        }

        public static BigDouble CriticalHitMultiplier()
        {
            return BASE_CRIT_MULTIPLIER + 
                MultiplyAllSources(BonusType.FLAT_CRIT_DMG);
        }

        public static bool ApplyCritHit(ref BigDouble val)
        {
            BigDouble critChance = CriticalHitChance();

            if (critChance >= 1.0 || Random.Range(0.0f, 1.0f) < critChance)
            {
                val *= CriticalHitMultiplier();

                return true;
            }

            return false;
        }

        public static BigDouble BaseMercDamage(MercID merc)
        {
            GM.Mercs.Data.FullMercData data = App.Data.Mercs[merc];

            BigDouble baseDamage = data.Game.BaseDamage > 0 ? data.Game.BaseDamage : (data.Game.UnlockCost / (10.0f + BigDouble.Log10(data.Game.UnlockCost)));

            return Formulas.MercBaseDamage(baseDamage, data.User.Level);
        }

        public static BigDouble TotalMercDamage(MercID merc)
        {
            GM.Mercs.Data.FullMercData data = App.Data.Mercs[merc];

            BigDouble val = data.BaseDamage;

            val *= MultiplyAllSources(BonusType.MERC_DAMAGE);
            val *= MultiplyAllSources(data.Game.Attack.ToBonusType());

            return val;
        }

        public static BigInteger GetPrestigePoints(int stage)
        {
            BigDouble big = Formulas.CalcPrestigePoints(stage).ToBigDouble() * MultiplyAllSources(BonusType.MULTIPLY_PRESTIGE_BONUS);

            return BigInteger.Parse(big.Ceiling().ToString("F0"));
        }

        public static BigDouble GetTapDamage()
        {
            return (Formulas.CalcTapDamage() * MultiplyAllSources(BonusType.TAP_DAMAGE)) + App.Data.Mercs.CalcTapDamageBonus();
        }


        // === Helper Methods ===

        static double MultiplyAllSources(BonusType type)
        {
            return (
                MultiplySource(type, ArmouryBonus) *
                MultiplySource(type, ArtefactBonus) *
                MultiplySource(type, CharacterBonus)
                );
        }

        static double AddAllSources(BonusType type)
        {
            return (
                AddSource(type, ArmouryBonus) +
                AddSource(type, ArtefactBonus) +
                AddSource(type, CharacterBonus)
                );
        }

        static double AddSource(BonusType type, List<KeyValuePair<BonusType, double>> ls)
        {
            double val = 0;

            foreach (KeyValuePair<BonusType, double> pair in ls)
            {
                if (pair.Key == type)
                {
                    val += pair.Value;
                }
            }

            return val;
        }

        static double MultiplySource(BonusType type, List<KeyValuePair<BonusType, double>> ls)
        {
            double val = 1;

            foreach (KeyValuePair<BonusType, double> pair in ls)
            {
                if (pair.Key == type)
                {
                    val *= pair.Value;
                }
            }

            return val;
        }
    }
}