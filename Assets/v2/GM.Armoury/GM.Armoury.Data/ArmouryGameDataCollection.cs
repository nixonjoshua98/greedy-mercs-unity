using SimpleJSON;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Armoury.Data
{
    public class ArmouryGameDataCollection
    {
        List<Models.ArmouryItemGameDataModel> itemsList;

        public ArmouryGameDataCollection(GM.Armoury.Models.AllArmouryGameDataModel data)
        {
            Update(data);
        }

        void Update(GM.Armoury.Models.AllArmouryGameDataModel data)
        {
            itemsList = data.Items;

            ScriptableObjects.LocalArmouryItemData[] allLocalArmouryItemData = LoadLocalData();

            foreach (var item in itemsList)
            {
                var localMerc = allLocalArmouryItemData.Where(ele => ele.Id == item.Id).First();

                item.Name = localMerc.Name;
                item.Icon = localMerc.Icon;

                item.EvoLevelCost = data.EvoLEvelCost;
                item.MaxEvolveLevel = data.MaxEvoLevel;
            }
        }


        public ArmouryGameDataCollection(JSONNode gameJSON)
        {
            UpdateFromJSON(gameJSON, gameJSON["items"]);
        }

        public Models.ArmouryItemGameDataModel GetItem(int key) => itemsList.Where(ele => ele.Id == key).FirstOrDefault();


        void UpdateFromJSON(JSONNode rootJSON, JSONNode itemsJSON)
        {
            //items = new Dictionary<int, Models.ArmouryItemGameDataModel>();

            //foreach (var ele in LoadLocalData())
            //{
            //    JSONNode currentItem = itemsJSON[ele.Id];

            //    items[ele.Id] = new Models.ArmouryItemGameDataModel
            //    {
            //        Id = ele.Id,
            //        Name = ele.Name,
            //        Icon = ele.Icon,

            //        EvoLevelCost = rootJSON["evoLevelCost"],
            //        MaxEvolveLevel = rootJSON["maxEvoLevel"],

            //        Tier = currentItem["itemTier"].AsInt,
            //        BaseDamage = currentItem["baseDamageMultiplier"].AsFloat
            //    };
            //}
        }


        static ScriptableObjects.LocalArmouryItemData[] LoadLocalData() => Resources.LoadAll<ScriptableObjects.LocalArmouryItemData>("Armoury/Items");
    }
}
