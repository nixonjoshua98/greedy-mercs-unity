using System;
using UnityEngine;
using UnityEngine.UI;

namespace GM.BountyShop.UI
{
    public class BountyShopUIController : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject ArmouryItemSlotObject;
        //public GameObject CurrencyItemSlotObject;

        [Header("References")]
        //public Text RefreshText;
        public Transform ItemsParent;

        private void Awake()
        {
            InstantiateItemSlots();
        }

        private void InstantiateItemSlots()
        {
            //App.BountyShop.CurrencyItems.ForEach(item => Instantiate<BountyShopCurrencyTypeSlot>(CurrencyItemSlotObject, ItemsParent).Assign(item));
            App.BountyShop.ArmouryItems.ForEach(item =>
            {
                Instantiate<BountyShopArmouryItemSlot>(ArmouryItemSlotObject, ItemsParent).Set(item);
             });
        }

        private void FixedUpdate()
        {
            //RefreshText.text = $"Daily Shop | <color=orange>{(App.DailyRefresh.Next - DateTime.UtcNow).Format()}</color>";
        }
    }
}