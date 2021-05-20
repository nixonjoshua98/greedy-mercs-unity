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

        public int gems;
        public long bountyPoints;
        public long armouryPoints;

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
            gems = node.GetValueOrDefault("gems", 0).AsInt;

            bountyPoints = node.GetValueOrDefault("bountyPoints", 0).AsLong;
            armouryPoints = node.GetValueOrDefault("armouryPoints", 0).AsLong;

            prestigePoints = node.HasKey("prestigePoints") ? BigInteger.Parse(node["prestigePoints"], System.Globalization.NumberStyles.Any) : 0;
        }
    }
}
