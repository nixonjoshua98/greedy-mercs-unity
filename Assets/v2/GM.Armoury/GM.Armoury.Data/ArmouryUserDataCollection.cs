using SimpleJSON;
using System.Collections.Generic;
using System.Linq;

namespace GM.Armoury.Data
{
    public class ArmouryUserDataCollection
    {
        Dictionary<int, Models.UserArmouryItemModel> Items;

        public ArmouryUserDataCollection(JSONNode node)
        {
            UpdateWithJSON(node);
        }

        public Models.UserArmouryItemModel GetItem(int key) => Items[key];


        /// <summary>
        /// State list of the unlocked armoury items
        /// </summary>
        public List<Models.UserArmouryItemModel> OwnedItems => Items.Values.Where(ele => ele.NumOwned > 0).OrderBy(ele => ele.Id).ToList();

        /// <summary>
        /// Update dictionary with a JSON object
        /// </summary>
        public void UpdateWithJSON(JSONNode json)
        {
            Items = new Dictionary<int, Models.UserArmouryItemModel>();

            foreach (string key in json.Keys)
            {
                JSONNode current = json[key];

                int itemId = int.Parse(key);

                Items[itemId] = new Models.UserArmouryItemModel
                {
                    Id = itemId,
                    Level = current["level"].AsInt,
                    NumOwned = current["owned"].AsInt,
                    EvoLevel = current["evoLevel"].AsInt
                };
            }
        }

        public void UpdateItemsWithModel(Dictionary<int, Models.UserArmouryItemModel> items)
        {
            Items = items;
        }
    }
}
