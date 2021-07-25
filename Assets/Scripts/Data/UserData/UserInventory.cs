using SimpleJSON;
using System.Numerics;


namespace GM.Data
{
    public class UserInventory
    {
        public BigInteger PrestigePoints;

        public int BountyPoints;
        public int IronIngots;
        public int BlueGems;

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
