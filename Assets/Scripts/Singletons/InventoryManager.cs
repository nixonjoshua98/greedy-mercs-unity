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

        public BigInteger prestigePoints;

        public int BlueGems;

        public int BountyPoints;
        public int ArmouryPoints;

        public static InventoryManager Create(JSONNode node)
        {
            Instance = new InventoryManager();

            Instance.Update(node);

            return Instance;
        }

        public void Update(JSONNode node)
        {
            SetItems(node["items"]);
        }

        public void SetItems(JSONNode node)
        {
            BlueGems = node.GetValueOrDefault("blueGems", 0).AsInt;

            BountyPoints = node.GetValueOrDefault("bountyPoints", 0).AsInt;
            ArmouryPoints = node.GetValueOrDefault("armouryPoints", 0).AsInt;

            prestigePoints = node.HasKey("prestigePoints") ? BigInteger.Parse(node["prestigePoints"], System.Globalization.NumberStyles.Any) : 0;
        }
    }
}
