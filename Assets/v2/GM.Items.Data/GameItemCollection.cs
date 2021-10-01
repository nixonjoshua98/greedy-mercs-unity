using System.Collections.Generic;

namespace GM.Items.Data
{
    public class GameItemCollection
    {
        Dictionary<ItemType, FullGameItemData> _internal;
        public GameItemCollection()
        {
            Update();
        }

        public FullGameItemData GetItem(ItemType item)
        {
            return _internal[item];
        }

        void Update()
        {
            _internal = new Dictionary<ItemType, FullGameItemData>();

            foreach (LocalItemData item in LoadLocalData())
            {
                _internal[item.Item] = new FullGameItemData
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
