using UnityEngine.Events;
using BigInteger = System.Numerics.BigInteger;

namespace GM.Inventory.Data
{
    public class UserInventoryCollection : Core.GMCache
    {
        public BigInteger PrestigePoints;

        public long BountyPoints;
        public long ArmouryPoints;

        public float Energy;

        public BigDouble Gold;

        // == Events == //
        public UnityEvent<long> E_BountyPointsChange = new UnityEvent<long>();
        public UnityEvent<BigInteger> E_PrestigePointsChange = new UnityEvent<BigInteger>();

        public UserInventoryCollection(Models.UserCurrenciesModel currencies)
        {
            Gold = 0;
            Energy = 0;

            UpdateCurrencies(currencies);
        }

        public void UpdateCurrencies(Models.UserCurrenciesModel model)
        {
            BountyPoints = model.BountyPoints;
            ArmouryPoints = model.ArmouryPoints;
            PrestigePoints = model.PrestigePoints;
        }
    }
}
