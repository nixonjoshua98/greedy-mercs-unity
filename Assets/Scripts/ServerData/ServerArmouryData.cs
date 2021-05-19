using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GM.Armoury
{
    public struct ArmouryItemData
    {
        public readonly int ID;

        public readonly int Tier;
        public readonly int UpgradeCost;
        public readonly int EvoUpgradeCost;

        public readonly float BaseDamageMultiplier;

        string iconString;

        public ArmouryItemData(int itemId, JSONNode node)
        {
            ID = itemId;

            Tier = node["itemTier"].AsInt;

            UpgradeCost = node["upgradeCost"].AsInt;
            EvoUpgradeCost = 10;

            BaseDamageMultiplier = node["damageBonus"].AsFloat;

            iconString = node.GetValueOrDefault("iconString", "default_armoury_item");
        }

        public Sprite Icon { get { return ResourceManager.LoadSprite("ArmourySprites", iconString); } }
    }

    public class ServerArmouryData
    {
        Dictionary<int, ArmouryItemData> items;

        public readonly int IronCost;

        public ServerArmouryData(JSONNode node)
        {
            IronCost = node["iron"]["cost"].AsInt;

            SetupItemData(node);
        }

        void SetupItemData(JSONNode node)
        {
            items = new Dictionary<int, ArmouryItemData>();

            JSONNode itemsNode = node["gear"];

            foreach (string key in itemsNode.Keys)
            {
                int id = int.Parse(key);

                ArmouryItemData data = new ArmouryItemData(id, itemsNode[key]);

                items.Add(id, data);
            }
        }

        // = = = GET = = =
        public List<ArmouryItemData> Items { get { return items.Values.ToList(); } }

        public ArmouryItemData Get(int itemId)
        {
            return items[itemId];
        }
    }
}