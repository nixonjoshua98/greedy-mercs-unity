﻿using UnityEngine.Events;
using BigInteger = System.Numerics.BigInteger;

namespace GM.Inventory.Data
{
    public class UserInventoryCollection
    {
        public BigInteger PrestigePoints;

        public long BountyPoints { get; private set; }
        public long ArmouryPoints { get; private set; }

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
