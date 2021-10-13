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
            foreach (var state in App.Data.Armoury.UserOwnedItems)
            {
                if (!itemObjects.ContainsKey(state.Id))
                {
                    InstantiateItem(state);
                }
            }
        }


        void FixedUpdate()
        {
            weaponPointText.text = App.Data.Inv.ArmouryPoints.ToString();
            damageBonusText.text = string.Format("{0}% Mercenary Damage", FormatString.Number(App.Cache.ArmouryMercDamageMultiplier * 100));
        }


        void InstantiateItem(Models.ArmouryItemUserDataModel state)
        {
            ArmouryItemSlot item = CanvasUtils.Instantiate<ArmouryItemSlot>(ArmouryItemObject, itemsParent);

            item.Init(state.Id);
            
            item.SetButtonCallback(() => { OnIconClick(state.Id); });

            itemObjects[state.Id] = item;
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