using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Armoury.Data
{
    public class ArmouryGameDataCollection
    {
        Dictionary<int, ArmouryItemGameData> DataDict;

        public ArmouryGameDataCollection(JSONNode gameJSON)
        {
            UpdateFromJSON(gameJSON, gameJSON["items"]);
        }

        public ArmouryItemGameData this[int key]
        {
            get => GetItem(key);
        }


        public ArmouryItemGameData GetItem(int key) => DataDict[key];


        void UpdateFromJSON(JSONNode rootJSON, JSONNode itemsJSON)
        {
            DataDict = new Dictionary<int, ArmouryItemGameData>();

            foreach (var ele in LoadLocalData())
            {
                JSONNode currentItem = itemsJSON[ele.ID];

                DataDict[ele.ID] = new ArmouryItemGameData
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


        static ScriptableObjects.LocalArmouryItemData[] LoadLocalData() => Resources.LoadAll<ScriptableObjects.LocalArmouryItemData>("Armoury/Items");
    }
}
