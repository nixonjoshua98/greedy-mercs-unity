using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

namespace RelicData
{
    public class RelicStaticData
    {
        public int maxLevel = 1_000;

        public BonusType bonusType;

        public int baseCost;
        public float costPower;

        public float baseEffect;
        public float levelEffect;
    }

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Container/RelicList")]
    public class RelicListSO : ScriptableObject
    {
        public List<RelicSO> RelicList;

        Dictionary<RelicID, RelicSO> RelicDict;

        public int Count { get { return RelicList.Count; } }

        public void Restore(JSONNode node)
        {
            RelicDict = new Dictionary<RelicID, RelicSO>();

            foreach (RelicSO scriptable in RelicList)
            {
                RelicStaticData data = JsonUtility.FromJson<RelicStaticData>(node[((int)scriptable.relic).ToString()].ToString());

                scriptable.Init(data);

                RelicDict[scriptable.relic] = scriptable;
            }
        }

        public RelicSO Get(RelicID relic) => RelicDict[relic];
    }
}