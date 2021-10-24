﻿using System.Numerics;

namespace GM.Inventory.Data
{
    public class UserInventoryCollection : Core.GMCache
    {
        public BigInteger PrestigePoints;

        public long BountyPoints;
        public long ArmouryPoints;

        public float Energy;

        public BigDouble Gold;

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
