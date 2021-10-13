using ArtefactData = GM.Artefacts.Data.ArtefactData;
using BigInteger = System.Numerics.BigInteger;
using TTLCache = GM.Common.TTLCache;

namespace GM.Core
{
    public class GMCache : GMClass
    {
        TTLCache cache;

        public GMCache()
        {
            cache = new TTLCache(3);
        }

        // == Armoury == //
        public double ArmouryMercDamageMultiplier => cache.Get<double>("ArmouryMercDamageMultiplier", () => App.Data.Armoury.TotalMercDamageMultiplier());

        // == Artefacts == //
        public BigInteger NextArtefactUnlockCost(int owned) => Formulas.NextArtefactUnlockCost(owned);
        public BigInteger ArtefactUpgradeCost(ArtefactData data, int levels)
        {
            // Cache value for longer since this calculation should not change and we can reuse it for multiple artefacts
            return cache.Get<BigInteger>($"ArtefactUpgradeCost/{data.CurrentLevel}{levels}{data.CostExpo}{data.CostCoeff}", 60, () => {
                return Formulas.ArtefactLevelUpCost(data.CurrentLevel, levels, data.CostExpo, data.CostCoeff);
                });
        }
    }
}
