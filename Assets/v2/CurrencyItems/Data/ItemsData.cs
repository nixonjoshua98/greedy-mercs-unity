using GM.Common.Enums;
using GM.CurrencyItems.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.CurrencyItems.Data
{
    public class ItemsData
    {
        Dictionary<CurrencyType, CurrencyItemScriptableObject> Items = new Dictionary<CurrencyType, CurrencyItemScriptableObject>();

        public ItemsData()
        {
            Update();
        }

        public CurrencyItemScriptableObject GetItem(CurrencyType item)
        {
            return Items[item];
        }

        void Update()
        {
            Items = LoadLocalData();
        }

        static Dictionary<CurrencyType, CurrencyItemScriptableObject> LoadLocalData() => Resources.LoadAll<CurrencyItemScriptableObject>("Scriptables/CurrencyItems").ToDictionary(x => x.Item, x => x);
    }
}