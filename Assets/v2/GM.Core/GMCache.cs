using BigInteger = System.Numerics.BigInteger;

namespace GM.Core
{
    public class GMCache : GMClass
    {
        GM.Common.TTLCache cache = new GM.Common.TTLCache();
        
        /// <summary>
        /// Merc damage multiplier from armoury
        /// </summary>
        public double ArmouryMercDamageMultiplier => cache.Get<double>($"ArmouryMercDamageMultiplier", 1, () => App.Data.Armoury.TotalMercDamageMultiplier());

        /// <summary>
        /// Unlock cost for next artefact
        /// </summary>
        public BigInteger ArtefactUnlockCost(int owned) => Formulas.Artefacts.UnlockCost(owned);

        /// <summary>
        /// Artefact upgrade cost. CurrentLevel -> (CurrentLevel + levels)
        /// </summary>
        public BigInteger ArtefactUpgradeCost(GM.Artefacts.Data.ArtefactData data, int levels) => 
            cache.Get<BigInteger>($"ArtefactUpgradeCost/{data.CurrentLevel}/{levels}/{data.CostExpo}/{data.CostCoeff}", 60, 
                () => Formulas.Artefacts.UpgradeCost(data.CurrentLevel, levels, data.CostExpo, data.CostCoeff));

        /// <summary>
        /// Upgrade cost for merc. CurrentLevel -> (CurrentLevel + levels)
        /// </summary>
        public BigDouble MercUpgradeCost(GM.Mercs.Data.FullMercData merc, int levels) => 
            cache.Get<BigDouble>($"MercUpgradeCost/{merc.CurrentLevel}/{levels}/{merc.UnlockCost}", 60, 
                () => BigMath.SumGeometricSeries(levels, merc.UnlockCost, 1.077, merc.CurrentLevel));

        /// <summary>
        /// Base merc damage for current level. Does not apply any bonuses
        /// </summary>
        public BigDouble MercBaseDamage(GM.Mercs.Data.FullMercData merc) => 
            cache.Get<BigDouble>($"MercBaseDamage/{merc.Id}/{merc.CurrentLevel}", 60, 
                () => Formulas.MercBaseDamage(merc.BaseDamage, merc.CurrentLevel));
    }
}