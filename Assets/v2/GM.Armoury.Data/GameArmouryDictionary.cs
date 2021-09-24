using SimpleJSON;
using System.Collections.Generic;

namespace GM.Armoury.Data
{
    public class GameArmouryDictionary : Dictionary<int, GM.Data.ArmouryItemData>
    {
        public GameArmouryDictionary(JSONNode gameJSON)
        {
            UpdateFromJSON(gameJSON);
        }


        void UpdateFromJSON(JSONNode node)
        {
            Clear();

            foreach (GM.Data.LocalArmouryItemData ele in LoadLocalData())
            {
                JSONNode current = node[ele.ID];

                base[ele.ID] = new GM.Data.ArmouryItemData()
                {
                    ID = ele.ID,
                    Name = ele.Name,
                    Icon = ele.Icon,

                    Tier = current["itemTier"].AsInt,
                    BaseDamageMultiplier = current["baseDamageMultiplier"].AsFloat
                };
            }
        }


        GM.Data.LocalArmouryItemData[] LoadLocalData() => UnityEngine.Resources.LoadAll<GM.Data.LocalArmouryItemData>("Armoury/Items");
    }
}
