using SRC.BountyShop.UI.ItemSlots;
using UnityEngine;
using TMPro;

namespace SRC.BountyShop.UI
{
    public class BountyShopTab : SRC.Core.GMMonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] GameObject ArmouryItemObject;
        [SerializeField] GameObject CurrencyItemObject;
        [Space]
        [SerializeField] TMP_Text BountyPointsText;

        [Header("Transforms")]
        [SerializeField] Transform ShopItemParent;

        public void Show()
        {
            gameObject.SetActive(true);

            App.BountyShop.FetchShop(() =>
            {
                InstantiateShopItems();
            });
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            ShopItemParent.DestroyChildren();
        }

        private void FixedUpdate()
        {
            BountyPointsText.text = Format.Number(App.Inventory.BountyPoints);
        }

        void InstantiateShopItems()
        {
            App.BountyShop.ArmouryItems.ForEach(aItem =>
            {
                var slot = this.Instantiate<BountyShopArmouryItemSlot>(ArmouryItemObject, ShopItemParent);

                slot.Initialize(aItem);
            });

            App.BountyShop.CurrencyItems.ForEach(aItem =>
            {
                var slot = this.Instantiate<BountyShopCurrencyItemSlot>(CurrencyItemObject, ShopItemParent);

                slot.Initialize(aItem);
            });

            ShopItemParent.ShuffleChildren(App.BountyShop.GameDayNumber);
        }
    }
}
