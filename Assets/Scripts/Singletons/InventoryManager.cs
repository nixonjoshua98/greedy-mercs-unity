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

        public int prestigePoints;

        public int BlueGems;

        public int BountyPoints;
        public int ArmouryPoints;

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

            BountyPoints = node.GetValueOrDefault("bountyPoints", 0).AsInt;
            ArmouryPoints = node.GetValueOrDefault("armouryPoints", 0).AsInt;

            prestigePoints = node.GetValueOrDefault("prestigePoints", 0).AsInt;
        }
    }
}
