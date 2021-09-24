using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Armoury.Data
{
    public class GameArmouryDictionary : Dictionary<int, ArmouryItemGameData>
    {
        public GameArmouryDictionary(JSONNode gameJSON)
        {
            UpdateFromJSON(gameJSON, gameJSON["items"]);
        }


        void UpdateFromJSON(JSONNode rootJSON, JSONNode itemsJSON)
        {
            Clear();

            foreach (LocalArmouryItemData ele in LoadLocalData())
            {
                JSONNode currentItem = itemsJSON[ele.ID];

                base[ele.ID] = new ArmouryItemGameData
                {
                    ID = ele.ID,
                    Name = ele.Name,
                    Icon = ele.Icon,

                    EvoLevelCost = rootJSON["evoLevelCost"],
                    MaxEvolveLevel = rootJSON["maxEvoLevel"],

                    Tier = currentItem["itemTier"].AsInt,
                    BaseDamage = currentItem["baseDamageMultiplier"].AsFloat
                };
            }
        }


        static LocalArmouryItemData[] LoadLocalData() => Resources.LoadAll<LocalArmouryItemData>("Armoury/Items");
    }
}
