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

        public float BaseDamageMultiplier;

        public Sprite Icon;
    }


    public class GameArmouryData
    {
        Dictionary<int, ArmouryItemData> items;

        public GameArmouryData(JSONNode node)
        {
            items = new Dictionary<int, ArmouryItemData>();

            foreach (LocalArmouryItemData ele in LoadLocalData())
            {
                if (node.TryGet(ele.Id.ToString(), out JSONNode svrItemData))
                {
                    items[ele.Id] = new ArmouryItemData()
                    {
                        Id = ele.Id,

                        Icon = ele.Icon,

                        Tier = svrItemData["itemTier"].AsInt,
                        BaseDamageMultiplier = svrItemData["baseDamageMultiplier"].AsFloat
                    };
                }
            }
        }

        public ArmouryItemData Get(int item) => items[item];

        LocalArmouryItemData[] LoadLocalData() => Resources.LoadAll<LocalArmouryItemData>("Armoury/Items");
    }
}