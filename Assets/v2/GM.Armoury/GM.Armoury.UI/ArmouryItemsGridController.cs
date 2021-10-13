using GM.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ScreenSpace = GM.UI_.ScreenSpace;

namespace GM.Armoury.UI_
{
    public class ArmouryItemsGridController : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject ArmouryItemSlotObject;

        [Header("References")]
        public GridLayoutGroup ItemsGridLayout;
        public Transform ItemsSlotsParent;

        [Header("Properties")]
        public int NumColumns = 3;

        Dictionary<int, ArmouryItemSlot> ItemSlots = new Dictionary<int, ArmouryItemSlot>();

        public void Populate(List<Models.ArmouryItemUserDataModel> items)
        {
            ItemSlots.Values.ToList().ForEach(obj => obj.gameObject.SetActive(false));

            for (int i = 0; i < items.Count; ++i)
            {
                var currentItem = items[i];

                ArmouryItemSlot slot = GetItemSlot(currentItem.Id);

                slot.gameObject.SetActive(true);
            }

            UpdateGridLayoutCellSize();
        }

        ArmouryItemSlot GetItemSlot(int itemId)
        {
            if (!ItemSlots.TryGetValue(itemId, out ArmouryItemSlot slot))
            {
                slot = GameObjectUtils.Instantiate<ArmouryItemSlot>(ArmouryItemSlotObject, ItemsGridLayout.transform);

                slot.AssignItem(itemId);

                //ItemSlots[itemId] = slot; // Cache the object so we don't recreate it for no reason
            }

            return slot;
        }

        void UpdateGridLayoutCellSize()
        {
            ItemsGridLayout.cellSize = new Vector3(ScreenSpace.Width / NumColumns, ItemsGridLayout.cellSize.y);

            ItemsGridLayout.CalculateLayoutInputHorizontal();
            ItemsGridLayout.CalculateLayoutInputVertical();
        }
    }
}
