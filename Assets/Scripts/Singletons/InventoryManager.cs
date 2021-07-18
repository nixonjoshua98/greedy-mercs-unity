using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

using BigInteger = System.Numerics.BigInteger;


namespace GM.Inventory
{
    public class InventoryManager
    {
        public static InventoryManager Instance = null;

        public int BlueGems;

        public int BountyPoints;
        public int IronIngots;

        public BigInteger PrestigePoints;

        InventoryManager(JSONNode node)
        {
            SetItems(node["items"]);
        }

        public static InventoryManager Create(JSONNode node)
        {
            Instance = new InventoryManager(node);

            return Instance;
        }

        public void SetItems(JSONNode node)
        {
            BlueGems = node.GetValueOrDefault("blueGems", 0).AsInt;

            BountyPoints    = node.GetValueOrDefault("bountyPoints", 0).AsInt;
            IronIngots      = node.GetValueOrDefault("ironIngots", 0).AsInt;

            if (node.HasKey("prestigePoints"))
            {
                PrestigePoints = BigInteger.Parse(node["prestigePoints"], System.Globalization.NumberStyles.Any);
            }
        }
    }
}
