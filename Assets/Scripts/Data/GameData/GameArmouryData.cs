using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GM.Data
{
    public struct ArmouryItemData
    {
        public int Id;

        public int Tier;

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
            ArmouryItemState state  = UserData.Get().Armoury.Get(itemId);

            return 5 + item.Tier + state.level;
        }


        public ArmouryItemData Get(int item) => items[item];


        void ParseItems(JSONNode node)
        {
            items = new Dictionary<int, ArmouryItemData>();

            foreach (LocalArmouryItemData ele in LoadLocalData())
            {
                if (node.TryGetKey(ele.Id, out JSONNode svrItemData))
                {
                    items[ele.Id] = new ArmouryItemData()
                    {
                        Id = ele.Id,
                        Name = ele.Name,
                        Icon = ele.Icon,

                        Tier = svrItemData["itemTier"].AsInt,
                        BaseDamageMultiplier = svrItemData["baseDamageMultiplier"].AsFloat
                    };
                }
            }
        }


        LocalArmouryItemData[] LoadLocalData() => Resources.LoadAll<LocalArmouryItemData>("Armoury/Items");
    }
}