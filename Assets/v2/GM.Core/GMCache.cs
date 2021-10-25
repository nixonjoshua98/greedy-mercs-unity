using ArtefactData = GM.Artefacts.Data.ArtefactData;
using BigInteger = System.Numerics.BigInteger;
using TTLCache = GM.Common.TTLCache;


namespace GM.Core
{
    public class GMCache : GMClass
    {
        TTLCache cache = new TTLCache();

        // == Armoury == //
        public double ArmouryMercDamageMultiplier => cache.Get<double>($"ArmouryMercDamageMultiplier", 1, () => App.Data.Armoury.TotalMercDamageMultiplier());

        // == Artefacts == //
        public BigInteger NextArtefactUnlockCost(int owned) => Formulas.NextArtefactUnlockCost(owned);
        public BigInteger ArtefactUpgradeCost(ArtefactData data, int levels)
        {
            // We cache the result using the method name and arguments as the key which *hopefully* will be unique forever
            return cache.Get<BigInteger>($"ArtefactUpgradeCost/{data.CurrentLevel}{levels}{data.CostExpo}{data.CostCoeff}", 60,
                () => Formulas.ArtefactLevelUpCost(data.CurrentLevel, levels, data.CostExpo, data.CostCoeff));
        }
    }
}