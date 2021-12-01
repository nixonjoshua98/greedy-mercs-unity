using System.Collections.Generic;
using System.Linq;
using BigInteger = System.Numerics.BigInteger;
using BonusType = GM.Common.Enums.BonusType;

namespace GM.Core
{
    public class GMCache : GMClass
    {
        // Temp
        public bool ApplyCritHit(ref BigDouble val)
        {
            float critChance = CriticalHitChance;

            if (MathUtils.PercentChance(critChance))
            {
                val *= CriticalHitMultiplier;

                return true;
            }

            return false;
        }


        GM.Common.TTLCache cache = new GM.Common.TTLCache();

        // = = Bonuses == //
        List<KeyValuePair<BonusType, double>> MercPassiveBonuses
        {
            get
            {
                List<KeyValuePair<BonusType, double>> ls = new List<KeyValuePair<BonusType, double>>();

                App.Data.Mercs.UnlockedMercs.ForEach(m => ls.AddRange(m.UnlockedPassives.Select(x => new KeyValuePair<BonusType, double>(x.Type, x.Value))));

                return ls;
            }
        }
        IEnumerable<KeyValuePair<BonusType, double>> ArtefactBonuses => App.Data.Arts.UserOwnedArtefacts.Select(s => new KeyValuePair<BonusType, double>(s.Bonus, s.BaseEffect));
        // == == = == == //


        Dictionary<BonusType, double> CombinedBonuses
        {
            get
            {
                return cache.Get<Dictionary<BonusType, double>>("CombinedBonuses", 1, () =>
                {
                    var ls = MercPassiveBonuses
                        .Concat(ArtefactBonuses);

                    return CreateBonusDictionary(ls);
                });
            }
        }


        public BigDouble GoldPerEnemyAtStage(int stage)
        {
            return Formulas.CalcEnemyGold(stage) * CombinedBonuses.Get(BonusType.ENEMY_GOLD, 1) * CombinedBonuses.Get(BonusType.ALL_GOLD, 1);
        }

        public BigDouble GoldPerStageBossAtStage(int stage)
        {
            return Formulas.CalcBossGold(stage) * CombinedBonuses.Get(BonusType.BOSS_GOLD, 1) * CombinedBonuses.Get(BonusType.ALL_GOLD, 1);
        }

        public BigInteger PrestigePointsForStage(int stage)
        {
            BigDouble big = Formulas.CalcPrestigePoints(stage).ToBigDouble() * CombinedBonuses.Get(BonusType.MULTIPLY_PRESTIGE_BONUS, 1);

            return big.FloorToBigInteger();
        }

        public float CriticalHitChance
        {
            get
            {
                return (float)(Constants.BASE_CRIT_CHANCE + CombinedBonuses.Get(BonusType.FLAT_CRIT_CHANCE, 0));
            }
        }

        public BigDouble CriticalHitMultiplier
        {
            get
            {
                return Constants.BASE_CRIT_MULTIPLIER + CombinedBonuses.Get(BonusType.FLAT_CRIT_DMG, 1);
            }
        }

        /// <summary>
        /// Merc damage multiplier from armoury
        /// </summary>
        public double ArmouryMercDamageMultiplier => cache.Get<double>($"ArmouryMercDamageMultiplier", 1, () => App.Data.Armoury.ArmouryMercDamageMultiplier);

        /// <summary>Unlock cost for next artefact</summary>
        public BigInteger ArtefactUnlockCost(int owned) => Formulas.ArtefactUnlockCost(owned);

        /// <summary>
        /// Artefact upgrade cost. CurrentLevel -> (CurrentLevel + levels)
        /// </summary>
        public BigInteger ArtefactUpgradeCost(GM.Artefacts.Data.ArtefactData data, int levels)
        {
            return cache.Get<BigInteger>($"ArtefactUpgradeCost/{data.CurrentLevel}/{levels}/{data.CostExpo}/{data.CostCoeff}", 60,
                () => Formulas.UpgradeCost(data.CurrentLevel, levels, data.CostExpo, data.CostCoeff));
        }

        /// <summary>
        /// Upgrade cost for merc. CurrentLevel -> (CurrentLevel + levels)
        /// </summary>
        public BigDouble MercUpgradeCost(GM.Mercs.Data.FullMercData merc, int levels)
        {
            return cache.Get<BigDouble>($"MercUpgradeCost/{merc.Id}/{merc.CurrentLevel}/{levels}", 60,
                () => Formulas.MercUpgradeCost(merc.CurrentLevel, levels, merc.UnlockCost));
        }

        /// <summary>
        /// Base merc damage for current level. Does not apply any bonuses
        /// </summary>
        public BigDouble MercBaseDamageAtLevel(GM.Mercs.Data.FullMercData merc)
        {
            return cache.Get<BigDouble>($"MercBaseDamageAtLevel/{merc.Id}/{merc.CurrentLevel}", 60,
                () => Formulas.MercBaseDamageAtLevel(merc.BaseDamage, merc.CurrentLevel));
        }

        public BigDouble MercDamage(GM.Mercs.Data.FullMercData merc)
        {
           return MercBaseDamageAtLevel(merc) * CombinedBonuses.Get(BonusType.MERC_DAMAGE, 1) * CombinedBonuses.Get(merc.AttackType.Bonus(), 1);
        }

        private Dictionary<BonusType, double> CreateBonusDictionary(IEnumerable<KeyValuePair<BonusType, double>> ls)
        {
            Dictionary<BonusType, double> result = new Dictionary<BonusType, double>();

            foreach (var pair in ls)
            {
                BonusType bonus = pair.Key;
                double value = pair.Value;

                if (!result.TryGetValue(bonus, out double totalValue))
                {
                    result[bonus] = value;
                }
                else
                {
                    result[bonus] = bonus switch
                    {
                        BonusType.FLAT_CRIT_CHANCE => totalValue + value,

                        _ => totalValue * value
                    };
                }
            }

            return result;
        }
    }
}