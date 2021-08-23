using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GM.Data
{
    public struct ArmouryItemData
    {
        public int ID;

        public int Tier { get; set; }

        public string Name;

        public float BaseDamageMultiplier;

        public Sprite Icon;
    }


    public class GameArmouryData
    {
        Dictionary<int, ArmouryItemData> items;

        public readonly int MaxEvolveLevel;
        public readonly int EvoLevelCost;

        public GameArmouryData(JSONNode node)
        {
            MaxEvolveLevel = node["maxEvoLevel"].AsInt;
            EvoLevelCost = node["evoLevelCost"].AsInt;

            ParseItems(node["items"]);
        }


        public int LevelCost(int itemId)
        {
            ArmouryItemData item    = Get(itemId);
            ArmouryItemState state  = UserData.Get.Armoury.Get(itemId);

            return 5 + (item.Tier + 1) + state.level;
        }


        public ArmouryItemData Get(int item)
        {
            return items[item];
        }


        void ParseItems(JSONNode node)
        {
            items = new Dictionary<int, ArmouryItemData>();

            foreach (LocalArmouryItemData ele in LoadLocalData())
            {
                if (node.TryGetKey(ele.ID, out JSONNode result))
                {
                    items[ele.ID] = new ArmouryItemData()
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