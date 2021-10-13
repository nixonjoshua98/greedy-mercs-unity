using GM.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScreenSpace = GM.UI_.ScreenSpace;

namespace GM.Armoury.UI_
{
    public class ArmouryItemsGridController : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject ArmouryItemSlotObject;
        public GameObject ArmouryItemRowObject;

        [Header("References")]
        public GridLayoutGroup ItemsGridLayout;
        public Transform ItemsSlotsParent;

        [Header("Properties")]
        public int NumItemColumns = 3;

        Dictionary<int, ArmouryItemSlot> ItemSlots = new Dictionary<int, ArmouryItemSlot>();

        void Start()
        {
            UpdateGridLayoutCellSize();
        }

        public void Populate(List<Models.ArmouryItemUserDataModel> items)
        {
            for (int i = 0; i < items.Count; ++i)
            {
                var currentItem = items[i];

                GetItemSlot(currentItem.Id);
            }

            UpdateGridLayoutCellSize();
        }

        ArmouryItemSlot GetItemSlot(int itemId)
        {
            if (!ItemSlots.TryGetValue(itemId, out ArmouryItemSlot slot))
            {
                slot = GameObjectUtils.Instantiate<ArmouryItemSlot>(ArmouryItemSlotObject, ItemsGridLayout.transform);

                slot.AssignItem(itemId);

                //ItemSlots[itemId] = slot;
            }

            return slot;
        }

        void UpdateGridLayoutCellSize()
        {
            ItemsGridLayout.cellSize = new Vector3(ScreenSpace.Width / NumItemColumns, ItemsGridLayout.cellSize.y);

            ItemsGridLayout.CalculateLayoutInputHorizontal();
            ItemsGridLayout.CalculateLayoutInputVertical();
        }
    }
}
