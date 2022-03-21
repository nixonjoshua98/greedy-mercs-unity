using System;
using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop.UI
{
    public class BountyShopUIController : GM.Core.GMMonoBehaviour
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
            App.BountyShop.CurrencyItems.ForEach(item => Instantiate<BountyShopCurrencyTypeSlot>(CurrencyItemSlotObject, ItemsParent).Assign(item));
            App.BountyShop.ArmouryItems.ForEach(item => Instantiate<BSArmouryItemSlot>(ItemSlotObject, ItemsParent).Assign(item));
        }

        void FixedUpdate()
        {
            RefreshText.text = $"Daily Shop | <color=orange>{(App.DailyRefresh.Next - DateTime.UtcNow).Format()}</color>";
        }
    }
}