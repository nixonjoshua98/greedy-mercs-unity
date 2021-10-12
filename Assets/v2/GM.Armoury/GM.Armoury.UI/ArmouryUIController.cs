using GM.Utils;
using UnityEngine;

namespace GM.Armoury.UI_
{
    public class ArmouryUIController : GM.UI.PanelController
    {
        [Header("Prefabs")]
        public GameObject ArmouryItemSlotObject;

        [Header("References")]
        public Transform ItemsSlotsParent;

        void Start()
        {
            foreach (var item in App.Data.Armoury.UserOwnedItems)
            {
                ArmouryItemSlot slot = GameObjectUtils.Instantiate<ArmouryItemSlot>(ArmouryItemSlotObject, ItemsSlotsParent);

                slot.AssignItem(item.Id);
            }

            foreach (var item in App.Data.Armoury.UserOwnedItems)
            {
                ArmouryItemSlot slot = GameObjectUtils.Instantiate<ArmouryItemSlot>(ArmouryItemSlotObject, ItemsSlotsParent);

                slot.AssignItem(item.Id);
            }
        }
    }
}
