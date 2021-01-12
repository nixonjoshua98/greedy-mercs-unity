using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

namespace RelicData
{    
    public class Relics
    {
        readonly Dictionary<RelicID, RelicStaticData> relics;

        public int Count { get { return relics.Count; } }

        public Dictionary<RelicID, RelicStaticData> All { get { return relics; } }

        public Relics(JSONNode node)
        {
            relics = new Dictionary<RelicID, RelicStaticData>();

            foreach (string key in node.Keys)
            {
                relics[(RelicID)int.Parse(key)] = JsonUtility.FromJson<RelicStaticData>(node[key].ToString());
            }
        }
    }
}
