using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GM.Armoury.UI
{
    using GM.UI;

    public class ArmouryPanel : PanelController
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

        protected override void OnShown()
        {
            foreach (Data.ArmouryItemState state in App.Data.Armoury.User.OwnedItems)
            {
                if (!itemObjects.ContainsKey(state.ID))
                {
                    InstantiateItem(state);
                }
            }
        }


        void FixedUpdate()
        {
            weaponPointText.text = App.Data.Inv.ArmouryPoints.ToString();
            damageBonusText.text = string.Format("{0}% Mercenary Damage", FormatString.Number(StatsCache.ArmouryMercDamageMultiplier * 100));
        }


        void InstantiateItem(Data.ArmouryItemState state)
        {
            ArmouryItemSlot item = CanvasUtils.Instantiate<ArmouryItemSlot>(ArmouryItemObject, itemsParent);

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