using System.Collections.Generic;

namespace GM.CurrencyItems.Data
{
    public class CurrencyItemsDataCollection
    {
        Dictionary<CurrencyType, CurrencyItemData> Items;

        public CurrencyItemsDataCollection()
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

            foreach (CurrencyItemLocalData item in LoadLocalData())
            {
                Items[item.Item] = new CurrencyItemData
                {
                    Icon = item.Icon,
                    DisplayName = item.DisplayName,
                    Item = item.Item
                };
            }
        }

        static CurrencyItemLocalData[] LoadLocalData() => UnityEngine.Resources.LoadAll<CurrencyItemLocalData>("Items");
    }
}
