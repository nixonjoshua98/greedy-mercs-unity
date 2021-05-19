using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GM.Armoury
{
    public struct ArmouryItemData
    {
        public int ID;

        public int Tier;

        public float BaseDamageMultiplier;

        string iconString;

        public ArmouryItemData(int itemId, JSONNode itemData)
        {
            ID = itemId;

            BaseDamageMultiplier    = itemData["baseDamageMultiplier"];
            Tier                    = itemData["itemTier"].AsInt;
            iconString              = itemData["iconString"];
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

            JSONNode itemsNode = node["items"];

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