using BigInteger = System.Numerics.BigInteger;
using UnityEngine;
using ArtefactData = GM.Artefacts.Data.ArtefactData;
namespace GM.Core
{
    public class GMCache : GMClass
    {
        public BigInteger NextArtefactUnlockCost(int owned) => (Mathf.Max(1, owned - 2) * BigDouble.Pow(1.35, owned).Floor()).ToBigInteger();
        public BigInteger ArtefactUpgradeCost(ArtefactData data, int levels) => Formulas.ArtefactLevelUpCost(data.CurrentLevel, levels, data.CostExpo, data.CostCoeff);
    }
}
