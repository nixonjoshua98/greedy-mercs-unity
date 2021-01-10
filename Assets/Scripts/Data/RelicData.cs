using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

namespace RelicData
{    
    public enum RelicID
    {
        CLICK_GUANTLET  = 0,
        IRON_SWORD      = 1,
        OLD_SHORTS      = 2,
        HP_POTION       = 3,
        LAST_RING       = 4,
        TAP_SCROLL      = 5,
        POWER_AXE       = 6,
        SPELL_TOME      = 7,
        BRACERS         = 8,
        LUCKY_GEM       = 9,
        JADE_NECKLACE   = 10,
        ARTEMIS_BOW     = 11,
        WEALTH_BAG      = 12,
        THIEF_BELT      = 13
    }

    public class RelicStaticData
    {
        public int maxLevel = 1_000;

        public BonusType bonusType;

        public int  baseCost;
        public float costPower;

        public float baseEffect;
        public float levelEffect;
    }

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
