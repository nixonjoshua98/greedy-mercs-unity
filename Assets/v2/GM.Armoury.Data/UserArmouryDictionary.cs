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

        public List<ArmouryItemState> OwnedItems => Values.Where(ele => ele.NumOwned > 0).OrderBy(ele => ele.ID).ToList();


        public void UpdateWithJSON(JSONNode node)
        {
            Clear();

            foreach (string key in node.Keys)
            {
                JSONNode current = node[key];

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
