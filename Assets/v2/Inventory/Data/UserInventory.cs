﻿using BigInteger = System.Numerics.BigInteger;

namespace GM.Inventory.Data
{
    public class UserInventory
    {
        public BigInteger PrestigePoints;

        public long BountyPoints { get; private set; }
        public long ArmouryPoints { get; private set; }

        public BigDouble Gold;

        public UserInventory(Models.UserCurrenciesModel currencies)
        {
            Gold = BigDouble.Billion;

            UpdateCurrencies(currencies);
        }

        public void UpdateCurrencies(Models.UserCurrenciesModel model)
        {
            BountyPoints = model.BountyPoints;
            ArmouryPoints = model.ArmouryPoints;
            PrestigePoints = model.PrestigePoints;
        }

        public void ResetLocalResources()
        {
            Gold = 0;
        }
    }
}