using SimpleJSON;
using System.Numerics;

namespace GM.Inventory.Data
{
    public class UserInventory
    {
        public BigInteger PrestigePoints;

        public long BountyP;
        public long ArmouryP;
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
            BlueGems        = node.GetValueOrDefault("blueGems", 0);
            ArmouryP      = node.GetValueOrDefault("armouryPoints", 0);
            BountyP    = node.GetValueOrDefault("bountyPoints", 0);

            PrestigePoints = BigInteger.Parse(node.GetValueOrDefault("prestigePoints", 0), System.Globalization.NumberStyles.Any);
        }
    }
}
