using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GM.Armoury.UI
{
    public class ArmouryItemsGridController : Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject ArmouryItemSlotObject;

        [Header("References")]
        public GM.UI.Layouts.ExpandableGridLayout ItemsGridLayout;
        public Transform ItemsSlotsParent;

        [Header("Properties")]
        public int NumColumns = 3;
        private readonly Dictionary<int, ArmouryItemSlot> ItemSlots = new Dictionary<int, ArmouryItemSlot>();

        public void Populate(List<AggregatedArmouryItem> items)
        {
            ItemSlots.Values.ToList().ForEach(obj => obj.gameObject.SetActive(false));

            for (int i = 0; i < items.Count; ++i)
            {
                var currentItem = items[i];

                ArmouryItemSlot slot = GetItemSlot(currentItem.ID);

                slot.gameObject.SetActive(true);
            }

            ItemsGridLayout.UpdateCellSize();
        }

        private ArmouryItemSlot GetItemSlot(int itemId)
        {
            if (!ItemSlots.TryGetValue(itemId, out ArmouryItemSlot slot))
            {
                slot = this.Instantiate<ArmouryItemSlot>(ArmouryItemSlotObject, ItemsGridLayout.transform);

                slot.AssignItem(itemId);

                ItemSlots[itemId] = slot; // Cache the object so we don't recreate it
            }

            return slot;
        }
    }
}
