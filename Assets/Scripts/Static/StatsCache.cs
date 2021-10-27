using System.Collections.Generic;
using System.Numerics;
using Random = UnityEngine.Random;

namespace GM
{
    public class StatsCache : Core.GMClass
    {
        const float BASE_CRIT_CHANCE = 0.01f;
        const float BASE_CRIT_MULTIPLIER = 2.5f;
        
        static List<KeyValuePair<BonusType, double>> ArmouryBonus => App.Data.Armoury.Bonuses();
        static List<KeyValuePair<BonusType, double>> ArtefactBonus => App.Data.Arts.Bonuses();
        static List<KeyValuePair<BonusType, double>> CharacterBonus => App.Data.Mercs.Bonuses();

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

        public static BigDouble TotalMercDamage(MercID merc)
        {
            GM.Mercs.Data.FullMercData data = App.Data.Mercs.GetMerc(merc);

            BigDouble val = App.Cache.MercBaseDamage(data);

            val *= MultiplyAllSources(BonusType.MERC_DAMAGE);
            val *= MultiplyAllSources(data.Attack.ToBonusType());

            return val;
        }

        public static BigInteger GetPrestigePoints(int stage)
        {
            BigDouble big = Formulas.CalcPrestigePoints(stage).ToBigDouble() * MultiplyAllSources(BonusType.MULTIPLY_PRESTIGE_BONUS);

            return BigInteger.Parse(big.Ceiling().ToString("F0"));
        }

        public static BigDouble GetTapDamage()
        {
            return Formulas.CalcTapDamage() * MultiplyAllSources(BonusType.TAP_DAMAGE);
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