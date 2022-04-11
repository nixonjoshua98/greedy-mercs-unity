using UnityEngine;

namespace GM.BountyShop.UI
{
    public class BountyShopUIController : GM.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject ArmouryItemSlotObject;
        public GameObject CurrencyItemSlotObject;

        [Header("References")]
        public Transform ItemsParent;

        private void Awake()
        {
            InstantiateItemSlots();
        }

        private void InstantiateItemSlots()
        {
            App.BountyShop.CurrencyItems.ForEach(item =>
            {
                Instantiate<BountyShopCurrencyItemSlot>(CurrencyItemSlotObject, ItemsParent).Set(item);
            });

            App.BountyShop.ArmouryItems.ForEach(item =>
            {
                Instantiate<BountyShopArmouryItemSlot>(ArmouryItemSlotObject, ItemsParent).Set(item);
            });

            var rnd = GM.Common.Utility.SeededRandom(App.DailyRefresh.Previous.ToString());

            for (int i = 0; i < ItemsParent.childCount; i++)
            {
                int val = rnd.Next(0, ItemsParent.childCount);

                ItemsParent.GetChild(i).SetSiblingIndex(val);
            }

        }
    }
}