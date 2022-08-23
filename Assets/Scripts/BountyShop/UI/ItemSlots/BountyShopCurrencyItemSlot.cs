using SRC.BountyShop.Data;
using SRC.BountyShop.UI.BaseClasses;

namespace SRC.BountyShop.UI.ItemSlots
{
    public class BountyShopCurrencyItemSlot : BountyShopItemSlot<BountyShopCurrencyItem>
    {
        public override void BuyItem()
        {
            App.BountyShop.PurchaseCurrencyItem(ShopItem.ID, resp =>
            {
                UpdateSlotUI();
            });
        }

        protected override void UpdateUI()
        {
            Icon.Initialize(ShopItem.Item);

            NameText.text = $"{Format.Colour(ShopItem.Item.Colour, "[Currency]")} {ShopItem.Quantity}x {ShopItem.Item.DisplayName}";
            DescriptionText.text = ShopItem.Item.ShortDescription;
        }
    }
}
