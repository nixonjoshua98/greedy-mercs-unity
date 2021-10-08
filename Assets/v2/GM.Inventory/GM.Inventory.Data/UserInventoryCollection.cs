﻿using SimpleJSON;
using System.Numerics;

namespace GM.Inventory.Data
{
    public class UserInventoryCollection
    {
        public BigInteger PrestigePoints;

        public long BountyPoints;
        public long ArmouryPoints;
        public long BlueGems;

        public float Energy;

        public BigDouble Gold;

        public UserInventoryCollection(JSONNode node)
        {
            Gold = 0;
            Energy = 0;

            UpdateCurrenciesWithJSON(node["items"]);
        }

        public UserInventoryCollection(Models.UserCurrenciesModel currencies)
        {
            Gold = 0;
            Energy = 0;

            UpdateCurrencies(currencies);
        }

        public void UpdateCurrenciesWithJSON(JSONNode node)
        {
            BlueGems = node.GetValueOrDefault("blueGems", 0);
            ArmouryPoints = node.GetValueOrDefault("armouryPoints", 0);
            BountyPoints = node.GetValueOrDefault("bountyPoints", 0);

            PrestigePoints = BigInteger.Parse(node.GetValueOrDefault("prestigePoints", 0), System.Globalization.NumberStyles.Any);
        }

        public void UpdateCurrencies(Models.UserCurrenciesModel model)
        {
            BlueGems = model.BlueGems;
            BountyPoints = model.BountyPoints;
            ArmouryPoints = model.ArmouryPoints;
            PrestigePoints = model.PrestigePoints;
        }
    }
}
