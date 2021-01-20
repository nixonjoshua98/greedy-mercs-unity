using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

namespace GreedyMercs
{
    public class LootStaticData
    {
        public int maxLevel = int.MaxValue;

        public BonusType bonusType;
        public ValueType valueType;

        public float costExpo;
        public float costCoeff;

        public float baseEffect;
        public float levelEffect;
    }

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Containers/LootList")]
    public class LootItemListSO : ScriptableObject
    {
        public List<LootItemSO> ItemList;

        Dictionary<LootID, LootItemSO> ItemDict;

        public int Count { get { return ItemList.Count; } }

        public void Restore(JSONNode node)
        {
            ItemDict = new Dictionary<LootID, LootItemSO>();

            foreach (LootItemSO scriptable in ItemList)
            {
                LootStaticData data = JsonUtility.FromJson<LootStaticData>(node[((int)scriptable.ItemID).ToString()].ToString());

                scriptable.Init(data);

                ItemDict[scriptable.ItemID] = scriptable;
            }
        }

        public LootItemSO Get(LootID relic) => ItemDict[relic];
    }
}