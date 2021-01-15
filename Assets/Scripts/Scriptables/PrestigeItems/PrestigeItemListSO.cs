using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

namespace LootData
{
    public class PrestigeItemStaticData
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
    [CreateAssetMenu(menuName = "Scriptables/Container/PrestigeItemsList")]
    public class PrestigeItemListSO : ScriptableObject
    {
        public List<LootItemSO> ItemList;

        Dictionary<LootID, LootItemSO> ItemDict;

        public int Count { get { return ItemList.Count; } }

        public void Restore(JSONNode node)
        {
            ItemDict = new Dictionary<LootID, LootItemSO>();

            foreach (LootItemSO scriptable in ItemList)
            {
                PrestigeItemStaticData data = JsonUtility.FromJson<PrestigeItemStaticData>(node[((int)scriptable.ItemID).ToString()].ToString());

                scriptable.Init(data);

                ItemDict[scriptable.ItemID] = scriptable;
            }
        }

        public LootItemSO Get(LootID relic) => ItemDict[relic];
    }
}