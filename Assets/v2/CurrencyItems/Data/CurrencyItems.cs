using System.Collections.Generic;
using GM.Common.Enums;

namespace GM.CurrencyItems.Data
{
    public class CurrencyItems
    {
        Dictionary<CurrencyType, CurrencyItemData> Items;

        public CurrencyItems()
        {
            Update();
        }

        public CurrencyItemData GetItem(CurrencyType item)
        {
            return Items[item];
        }

        void Update()
        {
            Items = new Dictionary<CurrencyType, CurrencyItemData>();

            foreach (var item in LoadLocalData())
            {
                Items[item.Item] = new CurrencyItemData
                {
                    Icon = item.Icon,
                    DisplayName = item.DisplayName,
                    Item = item.Item
                };
            }
        }

        static ScriptableObjects.CurrencyItemLocalData[] LoadLocalData() => UnityEngine.Resources.LoadAll<ScriptableObjects.CurrencyItemLocalData>("Items");
    }
}