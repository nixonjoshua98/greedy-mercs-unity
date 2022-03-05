using UnityEngine;
using UnityEngine.UI;
using System;

namespace GM.BountyShop.UI
{
    public class BountyShopUIController : GM.UI.Panels.Panel
    {
        [Header("Prefabs")]
        public GameObject ItemSlotObject;
        public GameObject CurrencyItemSlotObject;

        [Header("References")]
        public Text RefreshText;
        public Transform ItemsParent;

        void Awake()
        {
            InstantiateItemSlots();
        }

        void InstantiateItemSlots()
        {
            App.GMData.BountyShop.CurrencyItems.ForEach(item => Instantiate<BountyShopCurrencyTypeSlot>(CurrencyItemSlotObject, ItemsParent).Assign(item));
            App.GMData.BountyShop.ArmouryItems.ForEach(item => Instantiate<BSArmouryItemSlot>(ItemSlotObject, ItemsParent).Assign(item));
        }

        void FixedUpdate()
        {
            RefreshText.text = $"Daily Shop | <color=orange>{(App.GMData.NextDailyReset - DateTime.UtcNow).Format()}</color>";
        }
    }
}