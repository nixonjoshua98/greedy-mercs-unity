using SimpleJSON;
using System.Collections.Generic;

namespace GM.Armoury.Data
{
    public class GameArmouryDictionary : Dictionary<int, ArmouryItemData>
    {
        public readonly int MaxEvolveLevel;
        public readonly int EvoLevelCost;

        public GameArmouryDictionary(JSONNode gameJSON)
        {
            MaxEvolveLevel = gameJSON["maxEvoLevel"].AsInt;
            EvoLevelCost = gameJSON["evoLevelCost"].AsInt;

            UpdateFromJSON(gameJSON["items"]);
        }


        void UpdateFromJSON(JSONNode itemsJSON)
        {
            Clear();

            foreach (GM.Data.LocalArmouryItemData ele in LoadLocalData())
            {
                JSONNode currentItem = itemsJSON[ele.ID];

                base[ele.ID] = new ArmouryItemData
                {
                    ID = ele.ID,
                    Name = ele.Name,
                    Icon = ele.Icon,

                    EvoLevelCost = EvoLevelCost,
                    MaxEvolveLevel = MaxEvolveLevel,

                    Tier = currentItem["itemTier"].AsInt,
                    BaseDamageMultiplier = currentItem["baseDamageMultiplier"].AsFloat
                };
            }
        }


        GM.Data.LocalArmouryItemData[] LoadLocalData() => UnityEngine.Resources.LoadAll<GM.Data.LocalArmouryItemData>("Armoury/Items");
    }
}
