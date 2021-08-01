using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Data
{
    public class GameItemData
    {
        Dictionary<ItemType, LocalItemData> items;

        public GameItemData()
        {
            items = new Dictionary<ItemType, LocalItemData>();

            foreach (LocalItemData item in LoadLocalData())
            {
                items[item.Item] = item;
            }
        }

        public LocalItemData Get(ItemType id) => items[id];

        static LocalItemData[] LoadLocalData() => Resources.LoadAll<LocalItemData>("Items");
    }
}
