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

            BaseDamageMultiplier    = itemData["baseDamageMultiplier"].AsFloat;
            Tier                    = itemData["itemTier"].AsInt;
            iconString              = itemData["iconString"];
        }

        public Sprite Icon { get { return ResourceManager.LoadSprite("ArmourySprites", iconString); } }
    }

    public class ServerArmouryData
    {
        Dictionary<int, ArmouryItemData> items;

        public ServerArmouryData(JSONNode node)
        {
            SetupItemData(node);
        }

        void SetupItemData(JSONNode node)
        {
            items = new Dictionary<int, ArmouryItemData>();

            foreach (string key in node.Keys)
            {
                int id = int.Parse(key);

                ArmouryItemData data = new ArmouryItemData(id, node[key]);

                items.Add(id, data);
            }
        }

        // = = = GET = = =
        public ArmouryItemData Get(int itemId)
        {
            return items[itemId];
        }
    }
}