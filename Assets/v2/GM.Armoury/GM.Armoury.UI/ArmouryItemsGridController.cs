using GM.Utils;
using System.Collections.Generic;
using UnityEngine;
using ScreenSpace = GM.UI_.ScreenSpace;

namespace GM.Armoury.UI_
{
    public class ArmouryItemsGridController : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject ArmouryItemSlotObject;
        public GameObject ArmouryItemRowObject;

        [Header("References")]
        public Transform ItemsSlotsParent;

        [Header("Properties")]
        public int NumItemColumns = 3;

        Dictionary<int, ArmouryItemSlot> ItemSlots = new Dictionary<int, ArmouryItemSlot>();
        List<GameObject> ItemRows = new List<GameObject>();

        public void Populate(List<Models.ArmouryItemUserDataModel> items)
        {
            GameObject currentRow = null;

            for (int i = 0; i < items.Count; ++i)
            {
                var currentItem = items[i];

                if (i == 0 || i % NumItemColumns == 0)
                {
                    int indx = i / NumItemColumns;

                    currentRow = GetItemRow(indx);
                }

                ArmouryItemSlot slot = GetItemSlot(currentItem.Id, currentRow);

                RectTransform rt = slot.GetComponent<RectTransform>();

                rt.sizeDelta = new Vector2(ScreenSpace.Width / NumItemColumns, rt.sizeDelta.y);
            }
        }

        GameObject GetItemRow(int rowIndex)
        {
            GameObject row;

            if (ItemRows.Count > rowIndex)
            {
                row = ItemRows[rowIndex];
            }
            else
            {
                row = Instantiate(ArmouryItemRowObject, ItemsSlotsParent);

                ItemRows.Add(row);
            }

            return row;
        }

        ArmouryItemSlot GetItemSlot(int itemId, GameObject currentRow)
        {
            if (!ItemSlots.TryGetValue(itemId, out ArmouryItemSlot slot))
            {
                slot = GameObjectUtils.Instantiate<ArmouryItemSlot>(ArmouryItemSlotObject, currentRow.transform);

                slot.AssignItem(itemId);

                ItemSlots[itemId] = slot;
            }

            return slot;
        }
    }
}
