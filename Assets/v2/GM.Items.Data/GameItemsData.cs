using System.Collections.Generic;

namespace GM.Items.Data
{
    public class GameItemsData : Dictionary<ItemType, FullGameItemData>
    {
        public GameItemsData()
        {
            Update();
        }

        void Update()
        {
            foreach (LocalItemData item in LoadLocalData())
            {
                base[item.Item] = new FullGameItemData
                {
                    Icon = item.Icon,
                    DisplayName = item.DisplayName,
                    Item = item.Item
                };
            }
        }

        static LocalItemData[] LoadLocalData() => UnityEngine.Resources.LoadAll<LocalItemData>("Items");
    }
}
