using SimpleJSON;
using System.Numerics;

namespace GM.Inventory.Data
{
    public class UserInventory
    {
        public BigInteger PrestigePoints;

        public long BountyPoints;
        public long ArmouryPoints;
        public long BlueGems;

        public float Energy;

        public BigDouble Gold;

        public UserInventory(JSONNode node)
        {
            Gold = 0;
            Energy = 0;

            UpdateCurrenciesWithJSON(node["items"]);
        }

        public void UpdateCurrenciesWithJSON(JSONNode node)
        {
            BlueGems = node.GetValueOrDefault("blueGems", 0);
            ArmouryPoints = node.GetValueOrDefault("armouryPoints", 0);
            BountyPoints = node.GetValueOrDefault("bountyPoints", 0);

            PrestigePoints = BigInteger.Parse(node.GetValueOrDefault("prestigePoints", 0), System.Globalization.NumberStyles.Any);
        }

        public void UpdateCurrencyItems(Models.UserCurrencies model)
        {
            BlueGems = model.BlueGems;
            BountyPoints = model.BountyPoints;
            ArmouryPoints = model.ArmouryPoints;
            PrestigePoints = model.PrestigePoints;
        }
    }
}
