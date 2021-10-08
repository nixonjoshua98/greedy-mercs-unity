using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using GM.Extensions;
using UnityEngine;

namespace GM.Armoury.Data
{
    public class ArmouryUserDataCollection
    {
        List<Models.UserArmouryItemModel> Items;

        public ArmouryUserDataCollection(JSONNode node)
        {
            UpdateWithJSON(node);
        }

        public ArmouryUserDataCollection(List<Models.UserArmouryItemModel> items)
        {
            Items = items;
        }

        public Models.UserArmouryItemModel GetItem(int key) => Items.Where(ele => ele.Id == key).FirstOrDefault();


        /// <summary>
        /// State list of the unlocked armoury items
        /// </summary>
        public List<Models.UserArmouryItemModel> OwnedItems => Items.Where(ele => ele.NumOwned > 0).OrderBy(ele => ele.Id).ToList();

        /// <summary>
        /// Update dictionary with a JSON object
        /// </summary>
        public void UpdateWithJSON(JSONNode json)
        {
            //Items = new List<Models.UserArmouryItemModel>();

            //foreach (string key in json.Keys)
            //{
            //    JSONNode current = json[key];

            //    int itemId = int.Parse(key);

            //    Items.Add(new Models.UserArmouryItemModel
            //    {
            //        Id = itemId,
            //        Level = current["level"].AsInt,
            //        NumOwned = current["owned"].AsInt,
            //        EvoLevel = current["evoLevel"].AsInt
            //    };)
            //}
        }

        public void Update(List<Models.UserArmouryItemModel> items)
        {
            Items = items;
        }


        public void Update(Models.UserArmouryItemModel item)
        {          
            Items.UpdateOrInsertElement(item, (ele) => ele.Id == item.Id);
        }
    }
}
