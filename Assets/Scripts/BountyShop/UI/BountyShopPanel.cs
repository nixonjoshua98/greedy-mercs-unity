using SRC.BountyShop.UI.BaseClasses;
using SRC.BountyShop.UI.ItemSlots;
using UnityEngine;

namespace SRC.BountyShop.UI
{
    public class BountyShopPanel : SRC.UI.PopupBase
    {
        [Header("Prefabs")]
        [SerializeField] GameObject ArmouryItemObject;
        [SerializeField] GameObject CurrencyItemObject;

        [Header("Transforms")]
        [SerializeField] Transform ShopItemParent;

        private void Awake()
        {
            if (!App.BountyShop.IsValid)
            {
                App.BountyShop.FetchShop(ShowShop);
            }
            else
            {
                ShowShop();
            }
        }

        void ShowShop()
        {
            InstantiateShopItems();
        }

        void InstantiateShopItems()
        {
            App.BountyShop.ArmouryItems.ForEach(aItem =>
            {
                var slot = this.Instantiate<BountyShopArmouryItemListSlot>(ArmouryItemObject, ShopItemParent);

                slot.Initialize(aItem);
            });

            App.BountyShop.CurrencyItems.ForEach(aItem =>
            {
                var slot = this.Instantiate<BountyShopCurrencyItemListSlot>(CurrencyItemObject, ShopItemParent);

                slot.Initialize(aItem);
            });

            ShopItemParent.ShuffleChildren();

            ShowInnerPanel();
        }
    }
}
