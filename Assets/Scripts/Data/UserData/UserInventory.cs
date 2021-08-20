using SimpleJSON;
using System.Numerics;


namespace GM.Data
{
    public class UserInventory
    {
        public BigInteger PrestigePoints;

        public long BountyPoints;
        public long IronIngots;
        public long BlueGems;

        public UserInventory(JSONNode node)
        {
            SetItems(node["items"]);
        }

        public void SetItems(JSONNode node)
        {
            BlueGems        = node.GetValueOrDefault("blueGems", 0).AsInt;
            IronIngots      = node.GetValueOrDefault("ironIngots", 0).AsInt;
            BountyPoints    = node.GetValueOrDefault("bountyPoints", 0).AsInt;

            PrestigePoints = BigInteger.Parse(node.GetValueOrDefault("prestigePoints", 0), System.Globalization.NumberStyles.Any);
        }
    }
}
