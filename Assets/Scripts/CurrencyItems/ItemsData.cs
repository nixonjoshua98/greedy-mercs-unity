using GM.Common.Enums;
using GM.CurrencyItems.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.CurrencyItems.Data
{
    public class ItemsData
    {
        private Dictionary<CurrencyType, CurrencyItemScriptableObject> Items = new Dictionary<CurrencyType, CurrencyItemScriptableObject>();

        public ItemsData()
        {
            Update();
        }

        public CurrencyItemScriptableObject GetItem(CurrencyType item)
        {
            return Items[item];
        }

        private void Update()
        {
            Items = LoadLocalData();
        }

        private static Dictionary<CurrencyType, CurrencyItemScriptableObject> LoadLocalData()
        {
            return Resources.LoadAll<CurrencyItemScriptableObject>("Scriptables/CurrencyItems").ToDictionary(x => x.Item, x => x);
        }
    }
}