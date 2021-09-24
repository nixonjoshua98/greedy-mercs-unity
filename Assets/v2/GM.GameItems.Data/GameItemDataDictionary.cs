using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.GameItems.Data
{
    using GM.Data;

    public class GameItemDataDictionary : Dictionary<ItemType, LocalItemData>
    {
        public GameItemDataDictionary()
        {
            foreach (LocalItemData item in LoadLocalData())
            {
                base[item.Item] = item;
            }
        }

        static GM.Data.LocalItemData[] LoadLocalData() => Resources.LoadAll<GM.Data.LocalItemData>("Items");
    }
}
