using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Armoury.UI
{
    using GM.Data;

    using GM.Armoury;
    using GM.Inventory;

    public class ArmouryPanel : MonoBehaviour
    {
        [Header("Compoents")]
        [SerializeField] Text damageBonusText;
        [SerializeField] Text weaponPointText;

        [SerializeField] Transform itemsParent;

        [Header("Prefabs")]
        [SerializeField] GameObject ArmouryItemObject;
        [SerializeField] GameObject ItemPopupObject;

        // ===
        Dictionary<int, ArmouryItemSlot> itemObjects;


        void Awake()
        {
            itemObjects = new Dictionary<int, ArmouryItemSlot>();
        }

        void OnEnable()
        {
            foreach (ArmouryItemState state in UserData.Get().Armoury.OwnedItems())
            {
                if (!itemObjects.ContainsKey(state.ID))
                {
                    InstantiateItem(state);
                }
            }
        }


        void FixedUpdate()
        {
            weaponPointText.text = InventoryManager.Instance.IronIngots.ToString();
            damageBonusText.text = string.Format("{0}% Mercenary Damage", FormatString.Number(StatsCache.ArmouryMercDamageMultiplier * 100));
        }


        void InstantiateItem(ArmouryItemState state)
        {
            ArmouryItemData data = GameData.Get().Armoury.Get(state.ID);

            ArmouryItemSlot item = CanvasUtils.Instantiate(ArmouryItemObject, itemsParent).GetComponent<ArmouryItemSlot>();

            item.Init(state.ID);
            
            item.SetButtonCallback(() => { OnIconClick(state.ID); });

            itemObjects[state.ID] = item;
        }

        // === Button Callback ===

        void OnIconClick(int itemId)
        {
            GameObject obj = CanvasUtils.Instantiate(ItemPopupObject);

            ArmouryItemPopup panel = obj.GetComponent<ArmouryItemPopup>();

            panel.Init(itemId);
        }
    }
}