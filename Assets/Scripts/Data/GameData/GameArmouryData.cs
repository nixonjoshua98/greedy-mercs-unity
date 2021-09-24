using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GM.Data
{
    public class GameArmouryData
    {
        Dictionary<int, GM.Armoury.Data.ArmouryItemData> items;

        public GameArmouryData(JSONNode node)
        {
            ParseItems(node["items"]);
        }


        public GM.Armoury.Data.ArmouryItemData Get(int item)
        {
            return items[item];
        }


        void ParseItems(JSONNode node)
        {
            items = new Dictionary<int, GM.Armoury.Data.ArmouryItemData>();

            foreach (LocalArmouryItemData ele in LoadLocalData())
            {
                if (node.TryGetKey(ele.ID, out JSONNode result))
                {
                    items[ele.ID] = new GM.Armoury.Data.ArmouryItemData()
                    {
                        ID = ele.ID,
                        Name = ele.Name,
                        Icon = ele.Icon,

                        Tier = result["itemTier"].AsInt,
                        BaseDamageMultiplier = result["baseDamageMultiplier"].AsFloat
                    };
                }
            }
        }


        LocalArmouryItemData[] LoadLocalData() => Resources.LoadAll<LocalArmouryItemData>("Armoury/Items");
    }
}