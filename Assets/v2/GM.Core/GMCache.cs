using BigInteger = System.Numerics.BigInteger;
using System.Collections.Generic;
using System.Linq;
using BonusType = GM.Common.Enums.BonusType;

namespace GM.Core
{
    public class GMCache : GMClass
    {
        GM.Common.TTLCache cache = new GM.Common.TTLCache();

        public Dictionary<BonusType, double> BonusesFromMercPassives(GM.Mercs.Data.FullMercData merc)
        {
            return SourceBonusProduct(merc.UnlockedPassives.Select(x => new KeyValuePair<BonusType, double>(x.Type, x.Value)));
        }
        
        /// <summary>
        /// Merc damage multiplier from armoury
        /// </summary>
        public double ArmouryMercDamageMultiplier => cache.Get<double>($"ArmouryMercDamageMultiplier", 1, () => App.Data.Armoury.ArmouryMercDamageMultiplier);

        /// <summary>
        /// Unlock cost for next artefact
        /// </summary>
        public BigInteger ArtefactUnlockCost(int owned) => Formulas.Artefacts.UnlockCost(owned);

        /// <summary>
        /// Artefact upgrade cost. CurrentLevel -> (CurrentLevel + levels)
        /// </summary>
        public BigInteger ArtefactUpgradeCost(GM.Artefacts.Data.ArtefactData data, int levels)
        {
            return cache.Get<BigInteger>($"ArtefactUpgradeCost/{data.CurrentLevel}/{levels}/{data.CostExpo}/{data.CostCoeff}", 60,
                () => Formulas.Artefacts.UpgradeCost(data.CurrentLevel, levels, data.CostExpo, data.CostCoeff));
        }

        /// <summary>
        /// Upgrade cost for merc. CurrentLevel -> (CurrentLevel + levels)
        /// </summary>
        public BigDouble MercUpgradeCost(GM.Mercs.Data.FullMercData merc, int levels)
        {
            return cache.Get<BigDouble>($"MercUpgradeCost/{merc.CurrentLevel}/{levels}/{merc.UnlockCost}", 60,
                () => Formulas.Mercs.MercUpgradeCost(merc.CurrentLevel, levels, merc.UnlockCost));
        }

        /// <summary>
        /// Base merc damage for current level. Does not apply any bonuses
        /// </summary>
        public BigDouble MercBaseDamage(GM.Mercs.Data.FullMercData merc)
        {
            return cache.Get<BigDouble>($"MercBaseDamage/{merc.Id}/{merc.CurrentLevel}", 60,
                () => Formulas.MercBaseDamage(merc.BaseDamage, merc.CurrentLevel));
        }


        private Dictionary<BonusType, double> SourceBonusProduct(IEnumerable<KeyValuePair<BonusType, double>> ls)
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