using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Armoury.Data
{
    public class ArmouryGameDataCollection
    {
        Dictionary<int, Models.ArmouryItemGameDataModel> DataDict;

        public ArmouryGameDataCollection(JSONNode gameJSON)
        {
            UpdateFromJSON(gameJSON, gameJSON["items"]);
        }

        public Models.ArmouryItemGameDataModel this[int key]
        {
            get => GetItem(key);
        }


        public Models.ArmouryItemGameDataModel GetItem(int key) => DataDict[key];


        void UpdateFromJSON(JSONNode rootJSON, JSONNode itemsJSON)
        {
            DataDict = new Dictionary<int, Models.ArmouryItemGameDataModel>();

            foreach (var ele in LoadLocalData())
            {
                JSONNode currentItem = itemsJSON[ele.Id];

                DataDict[ele.Id] = new Models.ArmouryItemGameDataModel
                {
                    Id = ele.Id,
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
