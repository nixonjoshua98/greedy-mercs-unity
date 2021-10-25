using UnityEngine.Events;
using BigInteger = System.Numerics.BigInteger;

namespace GM.Inventory.Data
{
    public class UserInventoryCollection : Core.GMCache
    {
        #region long BountyPoints
        long _BountyPoints;
        public long BountyPoints
        {
            get => _BountyPoints;
            set
            {
                var change = value - _BountyPoints;

                _BountyPoints = value;

                if (change != 0)
                    E_BountyPointsChange.Invoke(change);
            }
        }
        #endregion

        #region BigInteger PrestigePoints
        BigInteger _PrestigePoints;
        public BigInteger PrestigePoints
        {
            get => _PrestigePoints;
            set
            {
                var change = value - _PrestigePoints;

                _PrestigePoints = value;

                if (change != 0)
                    E_PrestigePointsChange.Invoke(change);
            }
        }
        #endregion

        public long ArmouryPoints;

        public BigDouble Gold;

        // == Events == //
        public UnityEvent<long> E_BountyPointsChange = new UnityEvent<long>();
        public UnityEvent<BigInteger> E_PrestigePointsChange = new UnityEvent<BigInteger>();

        public UserInventoryCollection(Models.UserCurrenciesModel currencies)
        {
            Gold = 0;

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
