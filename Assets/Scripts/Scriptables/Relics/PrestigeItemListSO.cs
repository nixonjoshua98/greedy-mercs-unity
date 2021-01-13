using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

namespace PrestigeItemsData
{
    public class PrestigeItemStaticData
    {
        public int maxLevel = 1_000;

        public BonusType bonusType;

        public float costExpo;
        public float costCoeff;

        public float baseEffect;
        public float levelEffect;
    }

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Container/PrestigeItemsList")]
    public class PrestigeItemListSO : ScriptableObject
    {
        public List<PrestigeItemSO> ItemList;

        Dictionary<PrestigeItemID, PrestigeItemSO> ItemDict;

        public int Count { get { return ItemList.Count; } }

        public void Restore(JSONNode node)
        {
            ItemDict = new Dictionary<PrestigeItemID, PrestigeItemSO>();

            foreach (PrestigeItemSO scriptable in ItemList)
            {
                PrestigeItemStaticData data = JsonUtility.FromJson<PrestigeItemStaticData>(node[((int)scriptable.ItemID).ToString()].ToString());

                scriptable.Init(data);

                ItemDict[scriptable.ItemID] = scriptable;
            }
        }

        public PrestigeItemSO Get(PrestigeItemID relic) => ItemDict[relic];
    }
}