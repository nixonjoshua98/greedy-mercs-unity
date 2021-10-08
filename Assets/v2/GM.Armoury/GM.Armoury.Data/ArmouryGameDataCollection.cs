using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.Armoury.Data
{
    public class ArmouryGameDataCollection
    {
        List<Models.ArmouryItemGameDataModel> itemsList;

        public ArmouryGameDataCollection(Models.AllArmouryGameDataModel data)
        {
            Update(data);
        }

        void Update(Models.AllArmouryGameDataModel data)
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


        public Models.ArmouryItemGameDataModel GetItem(int key) => itemsList.Where(ele => ele.Id == key).FirstOrDefault();

        static ScriptableObjects.LocalArmouryItemData[] LoadLocalData() => Resources.LoadAll<ScriptableObjects.LocalArmouryItemData>("Armoury/Items");
    }
}
