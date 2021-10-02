using SimpleJSON;
using System.Collections.Generic;
using System.Linq;

namespace GM.Armoury.Data
{
    public class UserArmouryDictionary : Dictionary<int, ArmouryItemState>
    {
        public UserArmouryDictionary(JSONNode node)
        {
            UpdateWithJSON(node);
        }

        /// <summary>
        /// State list of the unlocked armoury items
        /// </summary>
        public List<ArmouryItemState> OwnedItems => Values.Where(ele => ele.NumOwned > 0).OrderBy(ele => ele.ID).ToList();

        /// <summary>
        /// Update dictionary with a JSON object
        /// </summary>
        public void UpdateWithJSON(JSONNode json)
        {
            Clear();

            foreach (string key in json.Keys)
            {
                JSONNode current = json[key];

                int itemId = int.Parse(key);

                base[itemId] = new ArmouryItemState(itemId)
                {
                    Level = current["level"].AsInt,
                    NumOwned = current["owned"].AsInt,
                    EvoLevel = current["evoLevel"].AsInt
                };
            }
        }
    }
}
