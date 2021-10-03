using SimpleJSON;
using System.Collections.Generic;
using System.Linq;

namespace GM.Armoury.Data
{
    public class ArmouryUserDataCollection
    {
        Dictionary<int, ArmouryItemState> Items;

        public ArmouryUserDataCollection(JSONNode node)
        {
            UpdateWithJSON(node);
        }

        public ArmouryItemState GetItem(int key) => Items[key];


        /// <summary>
        /// State list of the unlocked armoury items
        /// </summary>
        public List<ArmouryItemState> OwnedItems => Items.Values.Where(ele => ele.NumOwned > 0).OrderBy(ele => ele.Id).ToList();

        /// <summary>
        /// Update dictionary with a JSON object
        /// </summary>
        public void UpdateWithJSON(JSONNode json)
        {
            Items = new Dictionary<int, ArmouryItemState>();

            foreach (string key in json.Keys)
            {
                JSONNode current = json[key];

                int itemId = int.Parse(key);

                Items[itemId] = new ArmouryItemState
                {
                    Id = itemId,
                    Level = current["level"].AsInt,
                    NumOwned = current["owned"].AsInt,
                    EvoLevel = current["evoLevel"].AsInt
                };
            }
        }

        public void Update(Dictionary<int, ArmouryItemState> items)
        {
            Items = items;
        }
    }
}
