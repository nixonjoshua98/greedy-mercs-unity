using SRC.BountyShop.Data;
using SRC.BountyShop.UI.BaseClasses;

namespace SRC.BountyShop.UI.ItemSlots
{
    public class BountyShopCurrencyItemListSlot : BountyShopListSlot<BountyShopCurrencyItem>
    {
        public override void BuyItem()
        {
            App.BountyShop.PurchaseCurrencyItem(ShopItem.ID, resp =>
            {

            });
        }

        protected override void UpdateUI()
        {
            Icon.Initialize(ShopItem.Item);

            NameText.text = $"{Format.Colour(ShopItem.Item.Colour, "[Currency]")} {ShopItem.Item.DisplayName}";
        }
    }
}
