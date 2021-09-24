using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM.GameItems.Data
{
    public class GameItemsData
    {
        GameItemDataDictionary Items;

        public GameItemsData()
        {
            Items = new GameItemDataDictionary();
        }

        public GM.Data.LocalItemData this[GM.Data.ItemType key]
        {
            get => Items[key];
        }
    }
}
